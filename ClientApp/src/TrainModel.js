import React from 'react';
import { trainModel } from './api';

const TrainModel = () => {
    const handleTrainModel = async () => {
        try {
            const response = await trainModel();
            alert(response.data);
        } catch (error) {
            console.error('Model training failed', error);
        }
    };

    return (
        <div>
            <button onClick={handleTrainModel}>Train Model</button>
        </div>
    );
};

export default TrainModel;
