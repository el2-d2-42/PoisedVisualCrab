from flask import Flask, jsonify, request

app = Flask(__name__)

latest_gps_data = {
    "latitude": None,
    "longitude": None
}

@app.route('/')
def hello_world():
    return 'Hello, World!'

@app.route('/send-gps', methods=['POST'])
def receive_gps():
    data = request.json
    latitude = data.get('latitude')
    longitude = data.get('longitude')

    print("Received GPS data:")
    print(f"Latitude: {latitude}")
    print(f"Longitude: {longitude}")

    latest_gps_data["latitude"] = latitude
    latest_gps_data["longitude"] = longitude

    return jsonify(status="success", message="GPS data received")

@app.route('/get-gps', methods=['GET'])
def provide_gps():
    # Serve the latest stored GPS data
    return jsonify(latest_gps_data)

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)

