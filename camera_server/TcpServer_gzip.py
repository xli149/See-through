import socket
import cv2
import numpy as np
import time
import sys
import threading
import struct
import asyncio

import gzip
async def send_video(writer):
    # video_path = "innerscope.mp4"
    video_path = "/dev/video0"
    cap = cv2.VideoCapture(video_path)
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 320)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

    frame_rate = int(cap.get(cv2.CAP_PROP_FPS))

    if not cap.isOpened():
        print("Cannot open the video")
        exit()

    try:
        while True:
            ret, frame = cap.read()

            if not ret:
                break

            cv2.imshow('Video', frame)

            if cv2.waitKey(1000 // frame_rate) & 0xFF == ord('q'):
                break

            encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 5]
            frame_data = cv2.imencode('.jpg', frame, encode_param)[1].tobytes()
            test = gzip.compress(frame_data)
            # print(f"test: {len(test)}")
            # print(len(frame_data))
            data_to_send_binary = struct.pack('I', len(test))
            writer.write(data_to_send_binary)
            writer.write(test)

            await writer.drain()

        cap.release()
        writer.close()

    except Exception as e:
        print(f"Error: {str(e)}")

async def handle_client(reader, writer):
    video_task = asyncio.create_task(send_video(writer))

    try:
        await video_task
    except asyncio.CancelledError:
        pass

async def main():
    host = '0.0.0.0'
    port = 12345

    server = await asyncio.start_server(handle_client, host, port)

    print(f"Listening on {host}:{port}")

    try:
        async with server:
            await server.serve_forever()

    except KeyboardInterrupt:
        print("Server stopped by user")
        server.close()
        sys.exit(0)

if __name__ == "__main__":
    asyncio.run(main()) 