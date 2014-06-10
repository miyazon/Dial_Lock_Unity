using UnityEngine;
using System.Collections;

public class KeyR_Controller : MonoBehaviour
{
    public Transform target;

    GameObject dial;


    //ドラッグしている最中か否か
    public bool down = false;
    public float limit = 10.0f;

    private float _inertia = 0.0f;
    private float _prevX;
    private float _prevY;
    private Vector2 _delta = new Vector2(0.0f, 0.0f);



    void Start()
    {

        if (target == null)
        {
            target = transform;
        }
    }



    void Update()
    {
        //左クリックされた瞬間
        if (Input.GetMouseButtonDown(0))
        {
            //初期化
            _delta.x = 0.0f;
            _delta.y = 0.0f;
            _prevX = Input.mousePosition.x;
            _prevY = Input.mousePosition.y;
            down = true;


            //左クリックされたObjectがDial Rightか調べる
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Dial Right")
                {
                    down = true;

                }
                else
                {
                    down = false;
                }
            }

        }


        //左クリックをやめた瞬間
        if (Input.GetMouseButtonUp(0))
        {
            down = false;

            //_deltaの長さが８より大きい場合
            if (_delta.magnitude > 8.0f)
            {
                float v = Mathf.Clamp(_delta.sqrMagnitude, 0.0f, limit);
                _delta.Normalize();
                _delta *= v;
                _inertia = 1.0f;
            }
        }



        if (down)
        {
            //値の更新
            _delta.x = _prevX - Input.mousePosition.x;
            _delta.y = _prevY - Input.mousePosition.y;
            _prevX = Input.mousePosition.x;
            _prevY = Input.mousePosition.y;
            Vector3 aular = new Vector3(-_delta.y, 0.0f, 0.0f);
            //オブジェクトの角度更新
            target.Rotate(aular, Space.World);
        }
        else if (_inertia >= 0.0f)
        {
            _inertia *= 0.97f;

            if (_inertia > 0.05f)
            {
                Vector3 aular = new Vector3(-_delta.y * _inertia, 0.0f, 0.0f);
                //オブジェクトの角度更新
                target.Rotate(aular, Space.World);
            }
            else
            {
                _inertia = 0.0f;
            }
        }

    }


}
