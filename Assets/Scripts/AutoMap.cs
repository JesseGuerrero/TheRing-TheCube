using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Load mat
        Material mat = Resources.Load("Automat", typeof(Material)) as Material;   
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            //Get gameobj children
            GameObject child = gameObject.transform.GetChild(i).gameObject;

            //Apply Mat
            child.GetComponent<Renderer>().material = mat;
            
            //Set 2d objects inactive            
            string[] items = { "Dead_Bush", "Poppy", "Sunflower", "Grass", "Dandelion" };
            if(((IList<string>) items ).Contains(child.name)) child.SetActive(false);
            
            //Add a collider for the navmesh
            if(child.GetComponent<MeshCollider>() == null)
            {
                child.AddComponent<MeshCollider>();
            }                                    
        }
    }


}
