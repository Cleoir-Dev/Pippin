using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DevMod : MonoBehaviour
{
    [Header("Config Debugger")]
    public TilemapRenderer[] tilemapRenderers;
    public int floorLayer;
    public int utilLayer;
    public int orderLayer;
    
    // Start is called before the first frame update
    private void Awake()
    {
        // Verifica se o array tilemapRenderers n�o est� vazio
        if (tilemapRenderers != null && tilemapRenderers.Length > 0)
        {
            // Atribui a ordem de camada padr�o para todos os renderers
            foreach (var renderer in tilemapRenderers)
            {
                if (renderer != null)
                {
                    renderer.sortingOrder = orderLayer; // Usando sortingOrder para definir a ordem na camada
                }
            }

            // Atribui a ordem de camada espec�fica para os elementos espec�ficos do array
            if (tilemapRenderers.Length > 0 && tilemapRenderers[0] != null)
            {
                tilemapRenderers[0].sortingOrder = floorLayer;
            }

            if (tilemapRenderers.Length > 1 && tilemapRenderers[1] != null)
            {
                tilemapRenderers[1].sortingOrder = utilLayer;
            }
        }
        else
        {
            Debug.LogError("Array tilemapRenderers est� vazio ou n�o foi configurado.");
        }
    }
}
