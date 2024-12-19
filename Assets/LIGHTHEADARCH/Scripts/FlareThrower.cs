using System;

public interface IFlareThrower
{
    //FRANCO
    void ThrowFlare(); // Método para lanzar la bengala
    int GetFlareCount(); // Método para obtener el conteo de bengalas lanzadas
    void ResetFlareCount(); // Método para restablecer el contador de bengalas
}