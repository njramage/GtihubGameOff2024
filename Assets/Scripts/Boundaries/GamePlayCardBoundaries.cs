using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GamePlayCardBoundaries : MonoBehaviour
{
    public GameObject bottomLeftBoundary;
    public GameObject topRightBoundary;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    void Start() {
        bottomLeftLimit = bottomLeftBoundary.transform.position;
        topRightLimit = topRightBoundary.transform.position;
    }

    public Vector3 GetBottomLeftLimit() {
        return bottomLeftLimit;
    }

    public Vector3 GetTopRightLimit() {
        return topRightLimit;
    }
}
