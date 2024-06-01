import axios from 'axios';

const API_BASE_URL = 'http://localhost:44323/api'; // API base URL'inizi buraya yazın

export const predictStatus = (time, airline, destination) => {
    return axios.get(`${API_BASE_URL}/fetch-data`, {
        params: {
            time,
            airline,
            destination
        }
    });
};
