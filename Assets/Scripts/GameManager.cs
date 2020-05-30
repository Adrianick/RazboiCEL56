using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int NUMAR_CARTI = 52;
    private const int CATE_DE_UN_FEL = 4;
    private const int NUMAR_JUCATORI = 2;

    private CarteJoc[] pachetCarti = new CarteJoc[NUMAR_CARTI];

    private Queue<CarteJoc> cartiJucator1 = new Queue<CarteJoc>();
    private Queue<CarteJoc> cartiJucator2 = new Queue<CarteJoc>();
    private Queue<CarteJoc> cartiCastigate = new Queue<CarteJoc>();

    private GameObject go1;
    private GameObject go2;

    private bool cartiAfisate = false;
    private bool jocTerminat = false;
    public int modJoc = 0;
    private int cateRundeDeRazboiMare = 0;

    public Text valoareCarte1;
    public Text valoareCarte2;
    public Text numarCartiText1;
    public Text numarCartiText2;
    public Text textRazboiMare;

    public GameObject[] carti = new GameObject[52];

    [System.Serializable]
    public class CarteJoc
    {
        public string numeCarte;
        public int valoare;
        public GameObject go;
    }


    public CarteJoc[] cartiJoc = new CarteJoc[52];

    private Button buttonMod;

    void Start()
    {

        AmestecPachet();
        ImpartirePachet();

    }

    // Update is called once per frame
    void Update()
    {

        if (jocTerminat == false && Input.GetKeyDown("space") && cateRundeDeRazboiMare == 0)
        {
            Joc();
        }
        else if (cateRundeDeRazboiMare > 0 && jocTerminat == false)
        {
            if (Input.GetKeyDown("space"))
            {
                print("Runde Razboi mare ramase:" + cateRundeDeRazboiMare);
                ORundaDeRazboiMare();
                if (cateRundeDeRazboiMare == 0)
                {
                    textRazboiMare.text = "Razboiul mare s-a terminat";
                }
                else
                {
                    cateRundeDeRazboiMare--;
                    string s = "Razboi mare, carti ramase:" + (cateRundeDeRazboiMare);
                    textRazboiMare.text = s;
                }

            }
        }
    }

    void AmestecPachet()
    {
        for (int i = 0; i < NUMAR_CARTI; i++)
        {
            pachetCarti[i] = null;
        }


        for (int i = 0; i < NUMAR_CARTI; i++)
        {

            int pozitieCarte = Random.Range(0, 52);

            while (pachetCarti[pozitieCarte] != null)
            {
                pozitieCarte = Random.Range(0, 52);
            }


            pachetCarti[pozitieCarte] = cartiJoc[i];
        }

        //for (int i = 0; i < NUMAR_CARTI; i++)
        //{
        //    print("Cartea " + (i + 1) + " are valoarea: " + pachetCarti[i]);
        //}
    }

    void ImpartirePachet()
    {
        for (int i = 0; i < NUMAR_CARTI;)
        {
            cartiJucator1.Enqueue(pachetCarti[i++]);
            cartiJucator2.Enqueue(pachetCarti[i++]);
        }
    }

    void AfisareCarti(CarteJoc carte1, CarteJoc carte2)
    {
        if (cartiAfisate == true)
        {
            Destroy(go1);
            Destroy(go2);
        }

        go1 = Instantiate(carte1.go) as GameObject;
        go2 = Instantiate(carte2.go) as GameObject;

        // Setam pozitia lor

        go1.transform.position = new Vector3(-0.1f, 1f, -9.5f);
        go1.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        go1.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

        go2.transform.position = new Vector3(0.1f, 1f, -9.5f);
        go2.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        go2.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);



        var leftBlankCard = GameObject.FindGameObjectWithTag("BlankLeftCard").GetComponent<Animation>();

        UnityEditorInternal.ComponentUtility.CopyComponent(leftBlankCard);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(go1);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(go2);

        go1.GetComponent<Animation>().Play("BlankRotateLeftToRight");
        go2.GetComponent<Animation>().Play("BlankRotateRightToLeft");

        cartiAfisate = true;
    }

    void Joc()
    {
        // Fiecare pune cate o carte
        // Se compara cartile
        if (jocTerminat == true)
        {
            return;
        }
        CarteJoc carte1 = cartiJucator1.Dequeue();
        CarteJoc carte2 = cartiJucator2.Dequeue();

        AfisareCarti(carte1, carte2);

        cartiCastigate.Enqueue(carte1);
        cartiCastigate.Enqueue(carte2);

        valoareCarte1.text = carte1.valoare.ToString();
        valoareCarte2.text = carte2.valoare.ToString();

        numarCartiText1.text = cartiJucator1.Count.ToString();
        numarCartiText2.text = cartiJucator2.Count.ToString();

        if (carte1.valoare == carte2.valoare)
        {
            // RAZBOI MARE


            print("Razboi Mare:" + carte1.valoare + " vs " + carte1.valoare);

            cateRundeDeRazboiMare = carte1.valoare;

            string s = "Razboi mare, carti ramase:" + carte1.valoare;
            textRazboiMare.text = s;


            //bool razboiMare = RazboiMare(carte1);

            //if (razboiMare == true)
            //{
            //    return;
            //}
        }
        else
        {
            // RAZBOI MIC
            //print("Razboi Mic:" + carte1 + " vs " + carte2);
            if (carte1.valoare > carte2.valoare)
            {
                AmestecareCartiCastigate(1);
                if (cartiJucator2.Count == 0)
                {
                    Sfarsit(1);
                    return;
                }
            }
            else
            {
                AmestecareCartiCastigate(2);
                if (cartiJucator1.Count == 0)
                {
                    Sfarsit(2);
                    return;
                }
            }
        }
    }

    void ORundaDeRazboiMare()
    {
        if (cartiJucator1.Count == 0)
        {
            AmestecareCartiCastigate(2);
            Sfarsit(2);
            return;
        }
        else if (cartiJucator2.Count == 0)
        {
            AmestecareCartiCastigate(1);
            Sfarsit(1);
            return;
        }
        else if (modJoc == 1 && (cartiJucator1.Count == 1 || cartiJucator2.Count == 1))
        {
            print("Ultima carte din razboiul mare!");

            var carte1 = cartiJucator1.Dequeue();
            var carte2 = cartiJucator2.Dequeue();

            cartiCastigate.Enqueue(carte1);
            cartiCastigate.Enqueue(carte2);

            numarCartiText1.text = cartiJucator1.Count.ToString();
            numarCartiText2.text = cartiJucator2.Count.ToString();

            if (cartiJucator1.Count == 0)
            {
                if (carte1.valoare > carte2.valoare)
                {
                    AmestecareCartiCastigate(1);
                }
                else
                {
                    AmestecareCartiCastigate(2);
                    Sfarsit(2);
                }
            }
            else if (cartiJucator2.Count == 0)
            {
                if (carte2.valoare > carte1.valoare)
                {
                    AmestecareCartiCastigate(2);
                }
                else
                {
                    AmestecareCartiCastigate(1);
                    Sfarsit(1);
                }
            }
            cateRundeDeRazboiMare = 0;

            AfisareCarti(carte1, carte2);
            return;
        }

        var carte11 = cartiJucator1.Dequeue();
        var carte22 = cartiJucator2.Dequeue();

        cartiCastigate.Enqueue(carte11);
        cartiCastigate.Enqueue(carte22);

        AfisareCarti(carte11, carte22);

        numarCartiText1.text = cartiJucator1.Count.ToString();
        numarCartiText2.text = cartiJucator2.Count.ToString();

    }

    bool RazboiMare(int NUMAR_CARTIDeRazboi)
    {
        if (cartiJucator1.Count < NUMAR_CARTIDeRazboi)
        {

            for (int i = 0; i < cartiJucator1.Count; i++)
            {
                cartiCastigate.Enqueue(cartiJucator1.Dequeue());
                cartiCastigate.Enqueue(cartiJucator2.Dequeue());
            }
            AmestecareCartiCastigate(2);

            Sfarsit(2);
            return true;
        }
        if (cartiJucator2.Count < NUMAR_CARTIDeRazboi)
        {
            for (int i = 0; i < cartiJucator2.Count; i++)
            {
                cartiCastigate.Enqueue(cartiJucator1.Dequeue());
                cartiCastigate.Enqueue(cartiJucator2.Dequeue());
            }
            AmestecareCartiCastigate(1);

            Sfarsit(1);
            return true;
        }

        // se mai scot X - 1 carti, unde X = carte1, se compara ultimele doua intre ele 

        for (int i = 0; i < NUMAR_CARTIDeRazboi - 1; i++)
        {
            cartiCastigate.Enqueue(cartiJucator1.Dequeue());
            cartiCastigate.Enqueue(cartiJucator2.Dequeue());
        }

        Joc();

        return false;
    }

    void AmestecareCartiCastigate(int castigator)
    {
        //amestecam
        CarteJoc[] cartiPeMasa = new CarteJoc[cartiCastigate.Count];

        for (int i = 0; i < cartiCastigate.Count; i++)
        {
            cartiPeMasa[i] = null;
        }

        for (int i = 0; i < cartiPeMasa.Length; i++)
        {
            int pozitieCarte = Random.Range(0, cartiPeMasa.Length);

            while (cartiPeMasa[pozitieCarte] != null)
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

        numarCartiText1.text = cartiJucator1.Count.ToString();
        numarCartiText2.text = cartiJucator2.Count.ToString();

        print("Am amestecat cartile si le-am dat jucatorului: " + castigator);
        textRazboiMare.text = "";
    }

    void Sfarsit(int castigator)
    {
        jocTerminat = true;
        //Debug.Log("A castigat jucatorul " + castigator);
        print("A castigat jucatorul" + castigator);
    }
}
