using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int numarCarti = 52;
    private const int cateDeUnFel = 4;
    private const int numarJucatori = 2;

    private int[] pachetCarti = new int[numarCarti];

    private Queue<int> cartiJucator1 = new Queue<int>();
    private Queue<int> cartiJucator2 = new Queue<int>();
    private Queue<int> cartiCastigate = new Queue<int>();

    private bool JocTerminat = false;

    public Text valoareCarte1;
    public Text valoareCarte2;
    public Text numarCarti1;
    public Text numarCarti2;

    public GameObject[] carti = new GameObject[52];

    [System.Serializable]
    public class CarteJoc
    {
        public int numar;
        public int valoare;
        public GameObject go;
    }


    public CarteJoc[] cartiJoc = new CarteJoc[52];

    void Start()
    {

        AmestecPachet();
        ImpartirePachet();


    }

    // Update is called once per frame
    void Update()
    {
        if (JocTerminat == false && Input.GetKeyDown("space"))
        {
            Joc();
        }
    }

    void AmestecPachet()
    {
        for (int i = 0; i < numarCarti; i++)
        {
            pachetCarti[i] = 0;
        }

        int cartea = 1;
        int contor = 1;

        for (int i = 0; i < numarCarti; i++)
        {
            if (contor > 4)
            {
                cartea++;
                contor = 1;
            }

            int pozitieCarte = Random.Range(0, 52);

            while (pachetCarti[pozitieCarte] != 0)
            {
                pozitieCarte = Random.Range(0, 52);
            }


            pachetCarti[pozitieCarte] = cartea;

            contor++;
        }

        //for (int i = 0; i < numarCarti; i++)
        //{
        //    print("Cartea " + (i + 1) + " are valoarea: " + pachetCarti[i]);
        //}
    }

    void ImpartirePachet()
    {
        for (int i = 0; i < numarCarti;)
        {
            cartiJucator1.Enqueue(pachetCarti[i++]);
            cartiJucator2.Enqueue(pachetCarti[i++]);
        }
    }

    void Joc()
    {
        // Fiecare pune cate o carte
        // Se compara cartile
        if (JocTerminat == true)
        {
            return;
        }
        int carte1 = cartiJucator1.Dequeue();
        int carte2 = cartiJucator2.Dequeue();
        cartiCastigate.Enqueue(carte1);
        cartiCastigate.Enqueue(carte2);

        valoareCarte1.text = carte1.ToString();
        valoareCarte2.text = carte2.ToString();

        if (carte1 == carte2)
        {
            // RAZBOI MARE
            print("Razboi Mare:" + carte1);
            bool razboiMare = RazboiMare(carte1);

            if (razboiMare == true)
            {
                return;
            }
        }
        else
        {
            // RAZBOI MIC
            print("Razboi Mic:" + carte1 + " vs " + carte2);
            if (carte1 > carte2)
            {
                AmestecareCarti(1);
                if (cartiJucator2.Count == 0)
                {
                    Sfarsit(1);
                    return;
                }
            }
            else
            {
                AmestecareCarti(2);
                if (cartiJucator1.Count == 0)
                {
                    Sfarsit(2);
                    return;
                }
            }
        }
    }

    bool RazboiMare(int numarCartiDeRazboi)
    {
        if (cartiJucator1.Count < numarCartiDeRazboi)
        {

            for (int i = 0; i < cartiJucator1.Count; i++)
            {
                cartiCastigate.Enqueue(cartiJucator1.Dequeue());
                cartiCastigate.Enqueue(cartiJucator2.Dequeue());
            }
            AmestecareCarti(2);

            Sfarsit(2);
            return true;
        }
        if (cartiJucator2.Count < numarCartiDeRazboi)
        {
            for (int i = 0; i < cartiJucator2.Count; i++)
            {
                cartiCastigate.Enqueue(cartiJucator1.Dequeue());
                cartiCastigate.Enqueue(cartiJucator2.Dequeue());
            }
            AmestecareCarti(1);

            Sfarsit(1);
            return true;
        }

        // se mai scot X carti, unde X = carte1, se compara ultimele doua intre ele 

        for (int i = 0; i < numarCartiDeRazboi - 1; i++)
        {
            cartiCastigate.Enqueue(cartiJucator1.Dequeue());
            cartiCastigate.Enqueue(cartiJucator2.Dequeue());
        }

        Joc();

        return false;
    }

    void AmestecareCarti(int castigator)
    {
        //amestecam
        int[] cartiPeMasa = new int[cartiCastigate.Count];

        for (int i = 0; i < cartiCastigate.Count; i++)
        {
            cartiPeMasa[i] = 0;
        }

        for (int i = 0; i < cartiPeMasa.Length; i++)
        {
            int pozitieCarte = Random.Range(0, cartiPeMasa.Length);

            while (cartiPeMasa[pozitieCarte] != 0)
            {
                pozitieCarte = Random.Range(0, cartiPeMasa.Length);
            }

            cartiPeMasa[pozitieCarte] = cartiCastigate.Dequeue();
        }

        //impartim cartile castigatorului
        if (castigator == 1)
        {
            for (int i = 0; i < cartiPeMasa.Length; i++)
            {
                cartiJucator1.Enqueue(cartiPeMasa[i]);
            }
        }
        else
        {
            for (int i = 0; i < cartiPeMasa.Length; i++)
            {
                cartiJucator2.Enqueue(cartiPeMasa[i]);
            }
        }

        numarCarti1.text = cartiJucator1.Count.ToString();
        numarCarti2.text = cartiJucator2.Count.ToString();
    }

    void Sfarsit(int castigator)
    {
        JocTerminat = true;
        Debug.Log("A castigat jucatorul " + castigator);
        print("A castigat jucatorul" + castigator);
    }
}
