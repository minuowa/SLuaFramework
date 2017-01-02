using System;


public class Singleton<T> where T : new()
{
    protected static readonly T ms_instance = new T();

    protected Singleton()
	{
	}
	
	public static T Instance 
   	{ 
      	get  
      	{
         	return ms_instance;  
      	} 
   	}
}
