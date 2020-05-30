using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gm;

    public GameObject but;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            but.SetActive(false);
        }
    }

    public void SetMode()
    {

        gm.modJoc = 1;
    }
}