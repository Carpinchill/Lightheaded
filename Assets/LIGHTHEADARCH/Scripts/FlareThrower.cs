using System;

public interface IFlareThrower
{
    //FRANCO
    void ThrowFlare(); // M�todo para lanzar la bengala
    int GetFlareCount(); // M�todo para obtener el conteo de bengalas lanzadas
    void ResetFlareCount(); // M�todo para restablecer el contador de bengalas
}