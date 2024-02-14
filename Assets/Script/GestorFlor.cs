using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestorFlor : MonoBehaviour
{

    public int puntuacion = 0;
    public Flor[] listaFlor;


    private float timer = 0;
    public float tiempoEntreFlores;

    public int puntosAcierto = 10;
    public int puntosFallo = 10;

    public TextMeshPro textoPuntos;

    // si la partida acaba es true 
    public bool gameOver = false;

    //flores que aparecen en la ronda
    public int floresPorRonda = 10;

    //flores que han aparecido en la ronda
    private int floresSpawneados = 0;

    //referencia estatica parael singleton
    static public GestorFlor instance = null;

    public GameObject FinalPartida;

    //el awake se llama antes que el start
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ActualizaMarcador();
        listaFlor = GetComponentsInChildren<Flor>();
        FinalPartida.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            //ACTIVA UNA FLOR ENTRE FLORES SEGUNDOS
            timer += Time.deltaTime;
            if (timer >= tiempoEntreFlores)
            {
                //Debug.Log("llamando a activar flor");
                ActivaFlorAleatorio();
                timer = 0;

            }
            CompruebaFinDeRonda();
        }


        if (Input.GetMouseButtonDown(0))
        {

            PulsacionJugador();
        }


    }

    //activa un ade las flores al azar
    public void ActivaFlorAleatorio()
    {
      bool florEncontrada = false;
      while (!florEncontrada)
      { 
        //Obtengo un a posicion de la lista de topos 
        // Entre 0 y el tamaño de la lista
        int random = Random.Range(0, listaFlor.Length - 1);

        // compruebo que la flor este  inactivo antes de activarlo
       if(listaFlor[random].estado == EstadoFlor.INACTIVO)
       { 
                listaFlor[random].ActivarFlor(); //activo la flor en la posicion random
                florEncontrada = true;
                return;
       }
      }

        /*
        while (// mientras que  no encuentre un topo inactivo)
        {
             //busca un topo inactivo
             //si lo encuentras activalo

        }
        */
    }

    public void SumaPuntos()
    {

        Debug.Log("Puntos sumados");

    }

    public void PulsacionJugador()
    {
        
        //me guardo un ray entre la camara y la posicion del reaton
        // esto lo hago con el metodo screenpointtoray de la camara
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 4);

        RaycastHit infoRayo;
        
        if (Physics.Raycast(ray, out infoRayo))
        {
            if(infoRayo.transform.tag == "reset")
            {
                //he chocado con el boton reset
                ResetPartida();
                floresPorRonda = 10;
                FinalPartida.SetActive(false);
            }
            else //si no es un boton, compruebo si es una flor
            {
                /*Debug.Log("He chocado con " + infoRayo.transform.gameObject.name);
                Debug.Log("Estaba a una distancia de" + infoRayo.distance);
                Debug.Log("He chocado en la posicion" + infoRayo.point);
                Destroy(infoRayo.transform.gameObject);*/


                //compruebo si el objeto con el quie ha chocado el rayo
                //tiene el componente Flor y lo guardo en una variable
                Flor auxCompFlor = infoRayo.transform.gameObject.GetComponentInParent<Flor>();

                //si auxCompFlor existe (si he chocado con un topo)
                if (auxCompFlor != null)
                {
                    if (auxCompFlor.estado == EstadoFlor.APARECE || auxCompFlor.estado == EstadoFlor.ESPERA)
                    {
                        auxCompFlor.FlorGolpeada();
                        //Debug.Log("golpeada");
                    }
                }

            }
            

        }

    }

    private void ResetPartida()
    {
        puntuacion = 0;
        ActualizaMarcador();
        timer = 0;
        gameOver = false;
        floresSpawneados = 0;
        //for normal
        /*
        for( int i = 0; i < listaFlor.Length; i++)
        {
            listaFlor[i].ResetFlor();

        }
        ^*/

        foreach(Flor flor in listaFlor)
        {
            flor.ResetFlor();

        }
        
    }

    private void CompruebaFinDeRonda()
    {
        /* Rondas hasta tener x puntuacion
        
        if(puntuacion > 100)
        { 
            gameOver = true;
        
        }*/

        //Ronda hasta aparecer x topos

        if(floresSpawneados >= floresPorRonda)
        {
            gameOver = true;

        }


    }

    public void GanaPuntos()
    {
        if (puntuacion >= 0)
        {
            puntuacion += puntosAcierto;
            ActualizaMarcador();


        }
        if (puntuacion == 10)
        {
            floresPorRonda = 0;

            FinalPartida.SetActive(true);
        }

    }

    public void PierdePuntos()
    {
        if (puntuacion <= 0)
        {


        }
       else if(puntuacion >= 0)
       {
            puntuacion -= puntosFallo;
            ActualizaMarcador();
            
       }

    }

    public void ActualizaMarcador()
    {
        textoPuntos.text = puntuacion.ToString();


    }
}
