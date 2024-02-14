using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaRaycast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float resultadoCuenta;
        Division(100, 5, out resultadoCuenta);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public bool Division(float cociente, float divisor, out float resultado)
    {

        if (divisor == 0)
        {
            resultado = 0;
            return false;

        }
        resultado = cociente / divisor;

        return true;

    }

    public void PulsacionJugadorAll()
    {

        //me guardo un ray entre la camara y la posicion del reaton
        // esto lo hago con el metodo screenpointtoray de la camara
        Ray ray = new Ray(transform.position, Camera.main.ScreenToViewportPoint(Input.mousePosition));

        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        RaycastHit[] listaInfoRayo = Physics.RaycastAll(ray);

        if (listaInfoRayo.Length >= 0)
        {
            for(int i = 0; i<listaInfoRayo.Length; i++)
            {
                Debug.Log("He chocado con " + listaInfoRayo[i].transform.gameObject.name);
                Debug.Log("Estaba a una distancia de" + listaInfoRayo[i].distance);
                Debug.Log("He chocado en la posicion" + listaInfoRayo[i].point);
            }
           
            foreach(RaycastHit info in listaInfoRayo)
            {

                Destroy(info.transform.gameObject);
            }

        }

    }
}
