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
                <h1>Hello, world!</h1>
                <p>Welcome to your new single-page application, built with:</p>
                <ul>
                    <li>
                        <a href='https://get.asp.net/'>ASP.NET Core</a> and{' '}
                        <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>
                            C#
                        </a>{' '}
                        for cross-platform server-side code
                    </li>
                    <li>
                        <a href='https://facebook.github.io/react/'>React</a> for
                        client-side code
                    </li>
                    <li>
                        <a href='http://getbootstrap.com/'>Bootstrap</a> for layout and
                        styling
                    </li>
                </ul>
                <p>To help you get started, we have also set up:</p>
                <ul>
                    <li>
                        <strong>Client-side navigation</strong>. For example, click{' '}
                        <em>Counter</em> then <em>Back</em> to return here.
                    </li>
                    <li>
                        <strong>Development server integration</strong>. In development
                        mode, the development server from <code>create-react-app</code> runs
                        in the background automatically, so your client-side resources are
                        dynamically built on demand and the page refreshes when you modify
                        any file.
                    </li>
                    <li>
                        <strong>Efficient production builds</strong>. In production mode,
                        development-time features are disabled, and your{' '}
                        <code>dotnet publish</code> configuration produces minified,
                        efficiently bundled JavaScript files.
                    </li>
                </ul>
                <p>
                    The <code>ClientApp</code> subdirectory is a standard React
                    application based on the <code>create-react-app</code> template. If
                    you open a command prompt in that directory, you can run{' '}
                    <code>npm</code> commands such as <code>npm test</code> or{' '}
                    <code>npm install</code>.
                </p>

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