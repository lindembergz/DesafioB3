import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface EntradaInvestimentoDTO {
  valorInicial: number;
  quantidadeMeses: number;
}

interface ResultadoInvestimentoDTO {
  valorBruto: number;
  valorLiquido: number;
}

@Injectable({
  providedIn: 'root'
})
export class InvestimentoService {
  private apiUrl = 'http://localhost:5189/api/investimento/calcular-cdb';

  constructor(private http: HttpClient) { }

  calcularInvestimento(dados: EntradaInvestimentoDTO): Observable<ResultadoInvestimentoDTO> {
    return this.http.post<ResultadoInvestimentoDTO>(this.apiUrl, dados);
  }
}
