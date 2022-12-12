using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistenConnection : IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        public int retryCount = 0;
        private IConnection _connection;
        private bool _disposed;
        private object lock_object = new object();
        public RabbitMQPersistenConnection(IConnectionFactory connectionFactory, int _retryCount = 5)
        {
            this.retryCount = _retryCount;
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected)
            {
                TryConnect();
            }
        }
        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to dispose RabbitMQ connection", ex);
            }
        }

        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                        .Or<BrokerUnreachableException>()
                        .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                        {
                        });
                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += CallbackException;
                    _connection.ConnectionBlocked += ConnectionBlocked;
                    return true;
                }

                return false;
            }
        }

        private void CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (!_disposed)
            {
                TryConnect();
            }
        }
        private void ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (!_disposed)
            {
                TryConnect();
            }
        }
        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (!_disposed)
            {
                TryConnect();
            }
        }
    }
}
