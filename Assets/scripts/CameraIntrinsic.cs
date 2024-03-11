using UnityEngine;

public class CameraIntrinsic: MonoBehaviour
{
    // 相机内参
    public float fx; // x轴方向的焦距
    public float fy; // y轴方向的焦距
    public float cx; // x轴方向的光心坐标
    public float cy; // y轴方向的光心坐标

    // 畸变参数
    public float[] distortionCoefficients; // 畸变系数

    // 图像宽度和高度
    public int imageWidth;
    public int imageHeight;

    private void Start()
    {
        // 使用相机内参和畸变参数计算真实世界中的坐标差距

        // 计算图像中心点坐标
        float u = imageWidth / 2f;
        float v = imageHeight / 2f;

        // 根据图像中心点坐标计算归一化平面坐标
        float xNormalized = (u - cx) / fx;
        float yNormalized = (v - cy) / fy;

        // 根据归一化平面坐标计算射线方向
        Vector3 rayDirection = new Vector3(xNormalized, yNormalized, 1f).normalized;

        // 根据射线方向和畸变参数计算真实世界中的坐标差距
        Vector2 distortedPoint = DistortPoint(new Vector2(xNormalized, yNormalized));
        Vector3 worldOffset = new Vector3(distortedPoint.x, distortedPoint.y, 1f).normalized;

        // 输出真实世界中的坐标差距
        Debug.Log("World Offset: " + worldOffset);
    }

    // 畸变函数
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