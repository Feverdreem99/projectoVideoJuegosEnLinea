using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerTargetHud : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public RectTransform progressBarBg;
    public RectTransform heightIndicator; 
    public RectTransform opponentTarget;  
    public Image opponentTargetImage;     

    [Header("Configuración del Nivel")]
    public float alturaTotalDelNivel = 100f;

    [Header("Configuración del Radar (Ataque)")]
    public float distanciaRadar = 15f; 
    public float attackVerticalRange = 2f; 
    public Color targetMatchColor = Color.cyan; 
    
    private PlayerMovement localPlayer;
    private PlayerMovement opponentPlayer;
    private Color originalTargetColor;

    bool canMove = true;



    void Start()
    {
        if (opponentTargetImage != null) originalTargetColor = opponentTargetImage.color;
    }

    void Update()
    {
        

        if (localPlayer == null || opponentPlayer == null)
        {
            FindPlayersInScene();
            if (localPlayer == null || opponentPlayer == null) return;
        }

        UpdateHeightIndicator();
        UpdateOpponentTarget();
        CheckAttackProximity();
    }


    [PunRPC]
    void RPC_EndGame(string winnerName)
    {
        Debug.Log("Winner is: " + winnerName);

        // Disable movement, show UI, etc.
    }

    void WinGame()
    {
        photonView.RPC("RPC_EndGame", RpcTarget.All, PhotonNetwork.NickName);
    }

    void FindPlayersInScene()
    {
        PlayerMovement[] allPlayers = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement p in allPlayers)
        {
            if (p.photonView.IsMine) localPlayer = p;
            else opponentPlayer = p;
        }
    }

    void UpdateHeightIndicator()
    {
        float currentHeight = localPlayer.totalClimbedDistance; 
        float normalizedHeight = Mathf.Clamp01(currentHeight / alturaTotalDelNivel);
        float barHeight = progressBarBg.rect.height;

        float indicatorY = (normalizedHeight * barHeight) - (barHeight / 2f);
        heightIndicator.anchoredPosition = new Vector2(heightIndicator.anchoredPosition.x, indicatorY);
        
        Debug.Log(currentHeight);

        if (currentHeight >= alturaTotalDelNivel)
        {
            Debug.Log("Altura Maxima Alcanzada");
            WinGame();
           
        }
    }



    void UpdateOpponentTarget()
    {
        float heightDifference = opponentPlayer.totalClimbedDistance - localPlayer.totalClimbedDistance;
        
        float normalizedDiff = Mathf.Clamp(heightDifference / distanciaRadar, -1f, 1f); 
        float barHeight = progressBarBg.rect.height;
        
        float targetY = normalizedDiff * (barHeight / 2f); 
        opponentTarget.anchoredPosition = new Vector2(opponentTarget.anchoredPosition.x, targetY);

        if (opponentTargetImage != null)
        {
            Color c = opponentTargetImage.color;
            c.a = 0.3f + (Mathf.Sin(Time.time * 5f) + 1f) * 0.3f;
            opponentTargetImage.color = c;
        }
    }

    void CheckAttackProximity()
    {
        float heightDifference = Mathf.Abs(opponentPlayer.totalClimbedDistance - localPlayer.totalClimbedDistance);

        if (opponentTargetImage != null)
        {
            if (heightDifference <= attackVerticalRange)
            {
                opponentTargetImage.color = targetMatchColor; 
            }
            else
            {
                opponentTargetImage.color = originalTargetColor; 
            }
        }
    }
}