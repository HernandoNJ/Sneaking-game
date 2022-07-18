using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class XrCustomControllerInteractor : MonoBehaviour
{
    private XRBaseControllerInteractor _controller;

    private void Start()
    {
        _controller = GetComponent<XRBaseControllerInteractor>();
        Assert.IsNotNull(_controller, "There is no XRBaseController in hand " + gameObject.name);
        
        _controller.selectEntered.AddListener(ParentInteractable);
        _controller.selectExited.AddListener(UnParent);
    }

    private void ParentInteractable(SelectEnterEventArgs arg0)
    {
        arg0.interactable.transform.parent = transform;
    }

    private void UnParent(SelectExitEventArgs arg0)
    {
        arg0.interactable.transform.parent = null;
    }

}
