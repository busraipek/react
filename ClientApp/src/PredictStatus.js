import React, { useState } from 'react';
import axios from 'axios';
import './style.css'

function PredictStatus() {
    const [time, setTime] = useState('');
    const [airline, setAirline] = useState('');
    const [destination, setDestination] = useState('');
    const [predictedStatus, setPredictedStatus] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Convert time from HH:MM to a float representing the total minutes past midnight
        const [hours, minutes] = time.split(':').map(Number);
        const timeInMinutes = hours * 60 + minutes;

        try {
            const response = await axios.post('/api/prediction/predict', {
                Time: timeInMinutes,
                Airline: airline,
                Destination: destination
            });

            console.log('Prediction response:', response.data);

            setPredictedStatus(response.data.predictedStatus);
        } catch (error) {
            console.error('Error making prediction:', error);
        }
    };

    // Convert minutes to HH:MM format
    const formatTime = (minutes) => {
        const hours = Math.floor(minutes / 60);
        const mins = Math.floor(minutes % 60);
        return `${String(hours).padStart(2, '0')}:${String(mins).padStart(2, '0')}`;
    };

    return (
        <div>
            <h1>Predict Flight Status</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Time:</label>
                    <input type="time" value={time} onChange={(e) => setTime(e.target.value)} />
                </div>
                <div>
                    <label>Airline:</label>
                    <input type="text" value={airline} onChange={(e) => setAirline(e.target.value)} />
                </div>
                <div>
                    <label>Destination:</label>
                    <input type="text" value={destination} onChange={(e) => setDestination(e.target.value)} />
                </div>
                <button type="submit">Predict</button>
            </form>
            {predictedStatus !== null && (
                <div>
                    <p aria-live="polite">Predicted Status: <strong> {formatTime(predictedStatus)}</strong></p>
                </div>
            )}
        </div>
    );
}

export default PredictStatus;
