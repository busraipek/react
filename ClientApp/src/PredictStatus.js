import React, { useState } from 'react';
import axios from 'axios';

const PredictStatus = () => {
    const [time, setTime] = useState('');
    const [airline, setAirline] = useState('');
    const [destination, setDestination] = useState('');
    const [prediction, setPrediction] = useState(null);

    const handlePredict = async () => {
        try {
            const response = await axios.post('/api/prediction/predict', {
                time: parseFloat(time),
                airline,
                destination,
            });
            setPrediction(response.data.gecikme);
        } catch (error) {
            console.error('Error predicting delay:', error);
        }
    };

    return (
        <div>
            <h1>Flight Delay Prediction</h1>
            <div>
                <label>
                    Time:
                    <input
                        type="number"
                        value={time}
                        onChange={(e) => setTime(e.target.value)}

                    />
                </label>
            </div>
            <div>
                <label>
                    Airline:
                    <input
                        type="text"
                        value={airline}
                        onChange={(e) => setAirline(e.target.value)}
                    />
                </label>
            </div>
            <div>
                <label>
                    Destination:
                    <input
                        type="text"
                        value={destination}
                        onChange={(e) => setDestination(e.target.value)}
                    />
                </label>
            </div>
            <button onClick={handlePredict}>Predict Delay</button>
            {prediction !== null && (
                <div>
                    <h2>Predicted Delay: {prediction} minutes</h2>
                </div>
            )}
        </div>
    );
};

export default PredictStatus;
