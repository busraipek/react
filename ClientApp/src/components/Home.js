import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    componentDidMount() {
        this.interval = setInterval(() => {
            // iframe src attribute'ını güncelleyerek yeniden yükleyin
            const iframe = document.getElementById('avionio-frame');
            if (iframe) {
                const src = iframe.src;
                iframe.src = '';
                iframe.src = src;
            }
        }, 30 * 60 * 1000); // 30 dakika
    }

    componentWillUnmount() {
        // Zamanlayıcıyı temizle
        clearInterval(this.interval);
    }

    render() {
        return (
            <div>
                {/* Avionio Widget */}
                <div>
                    <div className="avionio-widget">
                        <iframe
                            id="avionio-frame"
                            height="650px"
                            frameBorder="0"
                            style={{ border: 0, width: '100%', margin: 0, padding: 0 }}
                            src="https://www.avionio.com/widget/en/saw/departures?style=2"
                        ></iframe>
                        <div style={{ fontSize: '12px', width: '100%', textAlign: 'center' }}>
                            <a
                                style={{ textDecoration: 'none', color: '#333' }}
                                href="https://www.avionio.com/en/airport/saw/departures"
                                title="SAW Departures"
                                target="_blank"
                                rel="noopener noreferrer"
                            >
                                SAW Departures
                            </a>
                            <span style={{ color: '#FD6150' }}>♥</span>
                            <a
                                style={{ textDecoration: 'none', color: '#333' }}
                                href="https://www.avionio.com/"
                                target="_blank"
                                rel="noopener noreferrer"
                                title="Airport arrivals and departures worldwide"
                            >
                                Avionio.com
                            </a>
                        </div>
                    </div>
                </div>
                {/* End of Avionio Widget */}
            </div>
        );
    }
}