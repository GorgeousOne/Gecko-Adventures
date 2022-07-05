using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] private GameObject mainMenu;
	[SerializeField] private GameObject optionsMenu;
	
	private PlayerControls _controls;

	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Back.performed += _ => GoBackInMenu();
		_controls.Enable();
	}
	
	private void OnDisable() {
		_controls.Disable();
	}
	
	public void PlayGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame() {
		Application.Quit();
		Debug.Log("Quit Game"); 
	}

	public void OpenOptions() {
		mainMenu.SetActive(false);
		DisplayMenu(optionsMenu);
	}

	public void CloseOptions() {
		optionsMenu.SetActive(false);
		DisplayMenu(mainMenu);
	}
	
	private void GoBackInMenu() {
		if (optionsMenu.activeSelf) {
			CloseOptions();
		}
	}

	private static void DisplayMenu(GameObject menu) {
		menu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(menu.GetComponentInChildren<Button>().gameObject);
	}
}
