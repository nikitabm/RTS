using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,Service {

	private List<GameObject> TeamOneUnits;
    private List<GameObject> TeamTwoUnits;
	private GameObject TeamOneController;
    private GameObject TeamTwoController;

	private EventManager _eventManager;
	private NetworkingManager _networkingManager;

    void Start () {
		ServiceLocator.ProvideService(_eventManager);
		ServiceLocator.ProvideService(_networkingManager);
		ServiceLocator.ProvideService(this);
		ServiceLocator.GetService();
	}
}
