using System.Runtime.InteropServices;

public class Inventaire
{
    private int nbViade;
    private int nbAlcool;

    public void SetViande(int _viande)
    {  
        nbViade = _viande; 
    }

    public int GetViande() 
    { 
        return nbViade; 
    }

    public void AddViande(int _viande)
    {
        nbViade += _viande;
    }

    public void SetAlcool(int _alcool)
    { 
        nbAlcool = _alcool; 
    }

    public int GetAlcool() 
    { 
        return nbAlcool; 
    }

    public void AddAlcool(int _alcool) 
    { 
        nbAlcool += _alcool; 
    }
}

