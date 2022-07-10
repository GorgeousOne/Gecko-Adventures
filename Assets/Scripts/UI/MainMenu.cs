using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] private GameObject mainMenu;
	// [SerializeField] private GameObject optionsMenu;
	
	private PlayerControls _controls;
	private GameObject _activeSubMenu;
	
	private void OnEnable() {
		_controls = new PlayerControls();
		_controls.Player.Back.performed += _ => CloseSubMenu();
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

	public void OpenSubMenu(GameObject subMenu) {
		CloseSubMenu();
		mainMenu.SetActive(false);
		_activeSubMenu = subMenu;
		DisplayMenu(_activeSubMenu);
	}
	
	public void CloseSubMenu() {
		if (_activeSubMenu != null) {
			_activeSubMenu.SetActive(false);
			DisplayMenu(mainMenu);
		}
	}

	private void DisplayMenu(GameObject menu) {
		menu.SetActive(true);
		EventSystem.current.SetSelectedGameObject(menu.GetComponentInChildren<Button>().gameObject);
		
	}
}
