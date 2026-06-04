using System.Collections;
using TMPro;
using UnityEngine;

public class VisualizadorCoordenadas : MonoBehaviour
{
    public TextMeshProUGUI textoCoordenadas;
    private bool servicioActivo = false;

    void Start()
    {
        StartCoroutine(IniciarServicioLocalizacion());
    }

    IEnumerator IniciarServicioLocalizacion()
    {
        if (!Input.location.isEnabledByUser)
        {
            textoCoordenadas.text = "Servicio de localización no habilitado por el usuario.";
            yield break;
        }

        Input.location.Start(1f, 1f); // Precisión de 1 metro, actualización cada 1 segundo

        int maxEspera = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxEspera > 0)
        {
            textoCoordenadas.text = "Inicializando servicio de localización...";
            yield return new WaitForSeconds(1);
            maxEspera--;
        }

        // Si tardó demasiado o falló la conexión
        if (maxEspera < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            textoCoordenadas.text = "No se pudo conectar con el GPS.";
            yield break;
        }

        servicioActivo = true;
    }

    void Update()
    {
        if (servicioActivo && Input.location.status == LocationServiceStatus.Running)
        {
            double latitud = Input.location.lastData.latitude;
            double longitud = Input.location.lastData.longitude;
            float altitud = Input.location.lastData.altitude;

            textoCoordenadas.text = $"Latitud: {latitud:F6}\nLongitud: {longitud:F6}\nAltitud: {altitud:F2} m";
        }
    }

    void OnDisable()
    {
        Input.location.Stop();
    }
}
