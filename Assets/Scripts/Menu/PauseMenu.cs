using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	[SerializeField] private GameObject background;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject optionsMenu;
	
	private PlayerControls _controls;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Menu.performed += _ => ToggleMenu();
		_controls.Player.Back.performed += _ => GoBackInMenu();
		_controls.Enable();
	}
	
	private void OnDisable() {
		_controls.Disable();
	}
	
	private void ToggleMenu() {
		if (!IsMenuOpen()) {
			OpenMenu();
		} else {
			CloseMenu();
		}
	}
	
	private void GoBackInMenu() {
		if (optionsMenu.activeSelf) {
			optionsMenu.SetActive(false);
			DisplayMenu(pauseMenu);
		} else {
			CloseMenu();
		}
	}

	public void OpenMenu() {
		Time.timeScale = 0.0f;
		background.SetActive(true);
		optionsMenu.SetActive(false);
		DisplayMenu(pauseMenu);
	}
	
	public void CloseMenu() {
		background.SetActive(false);
		pauseMenu.SetActive(false);
		optionsMenu.SetActive(false);
		Time.timeScale = 1.0f;
	}

	private bool IsMenuOpen() {
		return background.activeSelf;
	}
	
	public void OpenOptions() {
		pauseMenu.SetActive(false);
		DisplayMenu(optionsMenu);
	}

	public void CloseOptions() {
		optionsMenu.SetActive(false);
		DisplayMenu(pauseMenu);
	}
	
	public void QuitToMainMenu() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(0);
	}
	
	private static void DisplayMenu(GameObject menu) {
		menu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(menu.GetComponentInChildren<Button>().gameObject);
	}
}
