using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelDisplay : MonoBehaviour
{
    #region Variables
    [SerializeField] private MessageDisplay messageDisplay; // Reference to the message display component

    [Header("Panels")]
    [SerializeField] private GameObject confirmOversave; // Panel for confirming oversave

    [Header("Message text")]
    [SerializeField] private Text deleteMessage; // Text for delete confirmation message
    [SerializeField] private Text oversaveMessage; // Text for oversave confirmation message

    [Header("Inputs")]
    [SerializeField] private Dropdown weaponSelect; // Dropdown menu for selecting weapons
    [SerializeField] private InputWeaponPanel inputWeapon; // Input fields for weapon data
    #endregion


    #region Initialization
    private void Awake()
    {
        // Initialize weapon list and load selected weapon
        CreateWeaponList();
        weaponSelect.value = WeaponSave.LoadSelected();
        LoadSelected(weaponSelect.value);
    }

    private void CreateWeaponList()
    {
        // Populate dropdown menu with available weapons
        List<string> weapons = WeaponSave.GetWeaponList();
        weaponSelect.ClearOptions();
        weaponSelect.AddOptions(weapons);
    }

    public void ResetInput()
    {
        // Reset input fields for creating a new weapon
        inputWeapon.ResetInput();
    }
    #endregion


    #region CRUD Weapons
    public void TrySaveWeapon()
    {
        // Attempt to save the weapon, or prompt for oversaving if it already exists
        string weaponName = inputWeapon.weaponName.text;
        if (!WeaponSave.SaveExist(weaponName))
        {
            SaveWeapon();
        }
        else
        {
            ShowOversaveMessage(Messages.ConfirmOversave(weaponName));
        }
    }

    public void SaveWeapon()
    {
        // Save the weapon data
        try
        {
            WeaponModel weapon = inputWeapon.Serialize();
            WeaponSave.Save(weapon);

            // Update weapon list, select the saved weapon, and display a success message
            CreateWeaponList();
            weaponSelect.value = weaponSelect.options.FindIndex(o => o.text == weapon.weaponName);
            LoadWeapon(weaponSelect.value);
            SaveSelected();
            messageDisplay.ShowInfoMessage(Messages.InfoSaved(weapon.weaponName));
            // TODO: Show confirmation of save
        }
        catch (Exception e)
        {
            // Display an error message if saving fails
            messageDisplay.ShowErrorMessage(Messages.FileSaveError(inputWeapon.weaponName.text, e.Message));
        }
    }

    private void LoadWeapon(int weaponId)
    {
        // Load weapon data by its ID from the dropdown menu
        string weaponName = weaponSelect.options[weaponId].text;

        try
        {
            WeaponModel weapon = WeaponSave.Load(weaponName);
            inputWeapon.Deserialize(weapon);
            weaponSelect.value = weaponId;
        }
        catch (FileNotFoundException)
        {
            // Display an error message if the file is not found
            messageDisplay.ShowErrorMessage(Messages.FileNotFound(weaponName));
        }
        catch (Exception e)
        {
            // Display an error message if loading fails
            messageDisplay.ShowErrorMessage(Messages.FileReadError(e.Message));
        }
    }

    public void DeleteWeapon()
    {
        // Delete the selected weapon
        string weaponName = weaponSelect.options[weaponSelect.value].text;
        try
        {
            WeaponSave.Delete(weaponName);
            CreateWeaponList();
            weaponSelect.value = 0;
            messageDisplay.ShowInfoMessage(Messages.InfoDeleted(weaponName));
        }
        catch (FileNotFoundException)
        {
            // Display an error message if the file is not found
            messageDisplay.ShowErrorMessage(Messages.FileNotFound(weaponName));
        }
        catch (Exception e)
        {
            // Display an error message if deletion fails
            messageDisplay.ShowErrorMessage(Messages.FileReadError(e.Message));
        }
    }
    #endregion


    #region Message Handling
    private void ShowOversaveMessage(string message)
    {
        // Show the confirmation panel for oversaving
        confirmOversave.SetActive(true);
        oversaveMessage.text = message;
    }

    public void SetDeleteMessage()
    {
        // Set the delete confirmation message based on the selected weapon
        string weaponName = weaponSelect.options[weaponSelect.value].text;
        deleteMessage.text = Messages.ConfirmDelete(weaponName);
    }
    #endregion


    #region Current Weapon
    public void SaveSelected()
    {
        // Save the currently selected weapon
        int weaponId = weaponSelect.value;
        WeaponSave.SaveSelected(weaponId);
    }

    public void LoadSelected()
    {
        // Load the selected weapon
        if (weaponSelect.options.Count == 0)
        {
            return;
        }

        int selectedWeapon = weaponSelect.value;
        LoadSelected(selectedWeapon);
    }

    private void LoadSelected(int selectedWeapon)
    {
        // Load the selected weapon by its ID
        if (selectedWeapon != -1) LoadWeapon(selectedWeapon);
    }
    #endregion
}
