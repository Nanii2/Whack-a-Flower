using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoFlor { INACTIVO, APARECE, ESPERA, DESAPARECE };

public class Flor : MonoBehaviour
{
    public EstadoFlor estado = EstadoFlor.INACTIVO;

    //tiempos que pasa en cada estado antes de cambiar
    public float tAparece = 0.5f;
    public float tEspera = 1.5f;
    public float tDesaparece = 0.5f;

    public Vector3 escalaMin = Vector3.zero;
    public Vector3 escalaMax = new Vector3(1, 1, 1);

    public float zEnterrado = 0f;
    public float zDescubierto = 1f;

    private float timer = 0f;
    private GestorFlor gf;

    public AudioClip sfxGolpe;
    public AudioClip sfxFallo;

    public GameObject spriteAcierto;
    public GameObject spriteFallo;

    public ParticleSystem particulasGolpe;

    // Start is called before the first frame update
    void Start()
    {
        // guardamos referencias al gestor para abreviar el codigo
        // la guardamos en una variable de la clase, desde el instance (singleton)
        gf = GestorFlor.instance;
        spriteAcierto.SetActive(false);
        spriteFallo.SetActive(false);

    }

    private void OnEnable() // se llama cada vez que se activa este componente
    {

        ResetFlor(); // resetea

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch(estado)
        {

            //No hace  nada
            case EstadoFlor.INACTIVO:
                break;
            // Animacion de aparecer y pasa a esperar
            case EstadoFlor.APARECE:
                Apareciendo();
                break;
            //Espera un tiempo o hasta que le golpeen y cambia a desaparecer
            case EstadoFlor.ESPERA:
                Esperando();
                break;
            //Animacion de desaparecer y pasa a inactivo
            case EstadoFlor.DESAPARECE:
                Desapareciendo();
                break;
        }
    }

    void Apareciendo()
    {
        //Animacion de aparecer
        timer += Time.deltaTime;

        transform.localScale = Vector3.Lerp(escalaMin, escalaMax, timer / tAparece);

        //Animación aparecer con movimiento
        Vector3 auxPos = transform.localPosition;
        auxPos.z = Mathf.Lerp(zEnterrado, zDescubierto, timer / tAparece);
        transform.localPosition = auxPos;

        //comprobamos que ha pasado tAparecer
        if(timer >= tAparece)
        {
            //Pasar a esperar
            estado = EstadoFlor.ESPERA;
            timer = 0f;

        }

    }

    void Esperando()
    {
        timer += Time.deltaTime;

        //Mientras espera no hace nada

        //comprobamos que ha pasado tEspera
        if(timer >= tEspera)
        {
            gf.PierdePuntos();
            SoundManager.instance.PlaySFX(sfxFallo);
            //Pasar a desaparecer
            estado = EstadoFlor.DESAPARECE;
            //Reinicio el timer
            timer = 0f;
            spriteFallo.SetActive(true);
        }

        


    }
    void Desapareciendo()
    {
        //animacion de desaparecer
        timer += Time.deltaTime;

        //Animación desaparecer con movimiento
        Vector3 auxPos = transform.localPosition;
        auxPos.z = Mathf.Lerp(zDescubierto, zEnterrado, timer / tDesaparece);
        transform.localPosition = auxPos;

        transform.localScale = Vector3.Lerp(escalaMax, escalaMin, timer / tDesaparece);
        //comprobamos que ha pasado a tDesaparecer
        if (timer >= tDesaparece)
        {
            //pasar a inactivo
            estado = EstadoFlor.INACTIVO;
            //Reinicio el timer
            timer = 0f;
            spriteAcierto.SetActive(false);
            spriteFallo.SetActive(false);
        }

    }

    public void ResetFlor()
    {
        estado = EstadoFlor.INACTIVO;
        transform.localScale = escalaMin;
        Vector3 aux = transform.localPosition; 
        aux.z = zEnterrado;
        transform.localPosition = aux;
        timer = 0;
        spriteFallo.SetActive(false);
        spriteAcierto.SetActive(false);
        particulasGolpe.Clear(true);


    }

    public void ActivarFlor()
    {
        estado = EstadoFlor.APARECE;
        transform.localScale = escalaMin;
        Vector3 aux = transform.localPosition;
        aux.z = zEnterrado;
        transform.localPosition = aux;
        timer = 0;


    }

    public void FlorGolpeada()
    {

        estado = EstadoFlor.DESAPARECE;
        transform.localScale = escalaMax;
        Vector3 aux = transform.localPosition;
        aux.z = zDescubierto;
        transform.localPosition = aux;
        timer = 0;

        SoundManager.instance.PlayClip(sfxGolpe);
        gf.GanaPuntos();
        spriteAcierto.SetActive(true);

        particulasGolpe.Play();
        particulasGolpe.Emit(15);
    }

    
}
