using System.Runtime.InteropServices;

public class Inventory
{
    private int nbViande;
    private int nbAlcool;

    public void SetViande(int _viande)
    {  
        nbViande = _viande; 
    }

    public int GetViande() 
    { 
        return nbViande; 
    }

    public void AddViande(int _viande)
    {
        nbViande += _viande;
    }

    public void RemoveViande(int _viande)
    { 
        nbViande -= _viande;
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

    public void RemoveAlcool(int _alcool)
    {
        nbAlcool -= _alcool;
    }

    public void DisplayDetails()
    {
        Console.WriteLine($"Alcool: {nbAlcool}, Viande: {nbViande}");
    }
}

