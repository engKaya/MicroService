import { Injectable } from '@angular/core'; 
import { Buffer } from 'buffer';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {

    /**
     *
     */
    constructor( 
    ) {
        
    }

    GetToken(): string {
        var token = localStorage.getItem('token') || '';
        return token === '' ? '' : Buffer.from(token, "base64").toString('binary');
    }

    SetToken(token: string) {
        token = Buffer.from(token).toString('base64'); 
        localStorage.setItem('token', token);
    }

    RemoveToken() {
        localStorage.removeItem('token');
    }

    setUsername(username: string) { 
        username = Buffer.from(username).toString('base64'); 
        localStorage.setItem('username', username);
    }

    getUsername(): string {
        var username = localStorage.getItem('username') || '';
        return username === '' ? '' : Buffer.from(username, "base64").toString('binary');
    }

    RemoveUsername() {
        localStorage.removeItem('username');
    }

}