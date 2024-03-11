using UnityEngine;

public class CameraIntrinsic: MonoBehaviour
{
    // ����ڲ�
    public float fx; // x�᷽��Ľ���
    public float fy; // y�᷽��Ľ���
    public float cx; // x�᷽��Ĺ�������
    public float cy; // y�᷽��Ĺ�������

    // �������
    public float[] distortionCoefficients; // ����ϵ��

    // ͼ���Ⱥ͸߶�
    public int imageWidth;
    public int imageHeight;

    private void Start()
    {
        // ʹ������ڲκͻ������������ʵ�����е�������

        // ����ͼ�����ĵ�����
        float u = imageWidth / 2f;
        float v = imageHeight / 2f;

        // ����ͼ�����ĵ���������һ��ƽ������
        float xNormalized = (u - cx) / fx;
        float yNormalized = (v - cy) / fy;

        // ���ݹ�һ��ƽ������������߷���
        Vector3 rayDirection = new Vector3(xNormalized, yNormalized, 1f).normalized;

        // �������߷���ͻ������������ʵ�����е�������
        Vector2 distortedPoint = DistortPoint(new Vector2(xNormalized, yNormalized));
        Vector3 worldOffset = new Vector3(distortedPoint.x, distortedPoint.y, 1f).normalized;

        // �����ʵ�����е�������
        Debug.Log("World Offset: " + worldOffset);
    }

    // ���亯��
    private Vector2 DistortPoint(Vector2 point)
    {
        float k1 = distortionCoefficients[0];
        float k2 = distortionCoefficients[1];
        float k3 = distortionCoefficients[2];
        float p1 = distortionCoefficients[3];
        float p2 = distortionCoefficients[4];

        float r2 = point.x * point.x + point.y * point.y;
        float r4 = r2 * r2;
        float r6 = r2 * r4;

        float radialDistortion = 1f + k1 * r2 + k2 * r4 + k3 * r6;
        Vector2 tangentialDistortion = new Vector2(2f * p1 * point.x * point.y + p2 * (r2 + 2f * point.x * point.x),
                                                   p1 * (r2 + 2f * point.y * point.y) + 2f * p2 * point.x * point.y);

        return new Vector2(point.x * radialDistortion + tangentialDistortion.x,
                           point.y * radialDistortion + tangentialDistortion.y);
    }
}