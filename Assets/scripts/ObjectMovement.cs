using UnityEngine;
using Mirror;
public class ObjectMovement : NetworkBehaviour
{
    private bool isDragging = false; // �Ƿ�������ק����
    private Vector3 offset; // �����λ�ú��������ĵ�ƫ����

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            // ���������λ�ú��������ĵ�ƫ����
            offset = transform.position - GetMouseWorldPosition();

            // ��ʼ��ק����
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        // ֹͣ��ק����
        isDragging = false;
    }

    private void Update()
    {
        if (isDragging)
        {
            // ��ȡ��굱ǰλ��
            Vector3 mousePosition = GetMouseWorldPosition();
            //Debug.Log("mouse position: " + mousePosition);
            //Debug.Log("offset: " + offset);

            // Ӧ��ƫ��������������λ��
            transform.position = mousePosition + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        // �����λ�ô���Ļ����ת��Ϊ��������
        Vector3 mousePosition = Input.mousePosition;
        // �����λ��ת��Ϊ��������
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point;
        }

        // ���û�������κ����壬�򷵻�һ��Ĭ�ϵ�λ��
        return mouseRay.origin + mouseRay.direction * 10f;
    }
}