import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7044/chathub", {
    withCredentials: true,
  })
  .withAutomaticReconnect()
  .build();

export const startConnection = async () => {
  if (connection.state === signalR.HubConnectionState.Disconnected) {
    await connection.start();
    console.log("SignalR connected");
  }
};

export const stopConnection = async () => {
  if (connection.state === signalR.HubConnectionState.Connected) {
    await connection.stop();
    console.log("SignalR disconnected");
  }
};

export default connection;