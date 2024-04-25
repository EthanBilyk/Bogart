using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject depletedHeartPrefab;
    public Canvas playerUI; // Reference to the canvas GameObject in the Unity editor

    public Player player;
    private int hp;
    private int maxHP;

    void Start()
    {
        // Find the player object in the scene
        player = FindObjectOfType<Player>();

        // Call the method to display the player's health
        DisplayHP();
    }

    void Update()
    {
        // You can add any necessary update logic here
        if (hp != player.GetMaxHealth())
        {
            DisplayHP();
        }
    }

    public void DisplayHP()
    {
        // Clear existing heart images
        foreach (Transform child in playerUI.transform)
        {
            Destroy(child.gameObject);
        }

        hp = player.GetHealth();
        maxHP = player.GetMaxHealth();
        // Instantiate heart images based on player's health
        for (int i = 0; i < maxHP; i++)
        {
            // Instantiate the appropriate heart prefab as a child of the canvas
            GameObject heartPrefabToUse = (i < hp) ? heartPrefab : depletedHeartPrefab;
            GameObject heart = Instantiate(heartPrefabToUse, playerUI.transform);

            // Ensure the heart maintains the layout spacing
            LayoutElement layoutElement = heart.AddComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.flexibleWidth = 0;
                layoutElement.flexibleHeight = 0;
            }
        }

        // Ensure the layout updates
        HorizontalLayoutGroup layoutGroup = playerUI.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup != null)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.SetLayoutHorizontal();
        }
    }
}