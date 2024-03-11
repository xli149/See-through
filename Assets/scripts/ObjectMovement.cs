using UnityEngine;
using Mirror;
public class ObjectMovement : NetworkBehaviour
{
    private bool isDragging = false; // 是否正在拖拽物体
    private Vector3 offset; // 鼠标点击位置和物体中心的偏移量

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            // 计算鼠标点击位置和物体中心的偏移量
            offset = transform.position - GetMouseWorldPosition();

            // 开始拖拽物体
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        // 停止拖拽物体
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            // 获取鼠标当前位置
            Vector3 mousePosition = GetMouseWorldPosition();
            //Debug.Log("mouse position: " + mousePosition);
            //Debug.Log("offset: " + offset);

            // 应用偏移量并更新物体位置
            transform.position = mousePosition + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // 将鼠标位置从屏幕坐标转换为世界坐标
        Vector3 mousePosition = Input.mousePosition;
        // 将鼠标位置转换为世界坐标
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }

        // 如果没有命中任何物体，则返回一个默认的位置
        return mouseRay.origin + mouseRay.direction * 10f;
    }
}