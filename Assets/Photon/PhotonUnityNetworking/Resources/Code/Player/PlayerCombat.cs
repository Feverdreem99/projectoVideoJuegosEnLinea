using UnityEngine;
using Photon.Pun;

public class PlayerCombat : MonoBehaviourPun
{
    [Header("Configuración de Combate")]
    public int puntosPorGolpe = 30;
    public float cooldownGolpe = 0.5f; 

    [Header("Hitbox del Puñetazo")]
    public float distanciaGolpe = 1.5f; 
    public float alturaOffset = 1f;     
    public float radioGolpe = 1f;       
    
    private Animator animator; 
    private float tiempoUltimoGolpe = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("¡Ojo! No se encontró Animator en el Jugador o sus hijos.");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Time.time >= tiempoUltimoGolpe + cooldownGolpe)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LanzarGolpe(-1); // Izquierda
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                LanzarGolpe(1);  // Derecha
            }
        }
    }

    void LanzarGolpe(int direccion)
    {
        tiempoUltimoGolpe = Time.time;

        // --- 1. ACTIVAR ANIMACIÓN POR RED ---
        // Usamos un RPC para que ambos jugadores vean la animación sin fallos
        photonView.RPC("AnimarGolpeRPC", RpcTarget.All, direccion);

        // --- 2. LÓGICA DE DAÑO ---
        Vector3 offset = new Vector3(direccion * distanciaGolpe, alturaOffset, 0f);
        Vector3 centroDelGolpe = transform.position + offset;

        Collider[] objetosGolpeados = Physics.OverlapSphere(centroDelGolpe, radioGolpe);

        foreach (Collider tocado in objetosGolpeados)
        {
            ObstacleCollision obstaculo = tocado.GetComponentInParent<ObstacleCollision>();
            
            if (obstaculo != null && !obstaculo.hasHitPlayer && !obstaculo.wasPunched)
            {
                obstaculo.photonView.RPC("RecibirPunetazoRPC", RpcTarget.All);
                PlayerStats.LocalInstance.AddScore(puntosPorGolpe);
            }
        }
    }

    // Este RPC obliga al Animator de todos los jugadores en la partida a disparar el Trigger
    [PunRPC]
    public void AnimarGolpeRPC(int direccion)
    {
        if (animator != null)
        {
            if (direccion == -1) animator.SetTrigger("PunchLeft");
            else animator.SetTrigger("PunchRight");
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 offsetIzq = new Vector3(-1 * distanciaGolpe, alturaOffset, 0f);
        Vector3 offsetDer = new Vector3(1 * distanciaGolpe, alturaOffset, 0f);

        Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(transform.position + offsetIzq, radioGolpe);
        
        Gizmos.color = Color.red;  
        Gizmos.DrawWireSphere(transform.position + offsetDer, radioGolpe);
    }
}