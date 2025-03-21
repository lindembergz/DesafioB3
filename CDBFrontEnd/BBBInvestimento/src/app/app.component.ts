import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { RouterOutlet } from '@angular/router';
import { InvestimentoService } from './InvestimentoService';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, RouterOutlet], 
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [InvestimentoService] 
})
export class AppComponent {
  title = 'BBBInvestimento';

  valorInicial!: number;
  quantidadeMeses!: number;
  valorBruto?: number;
  valorLiquido?: number;
  erro?: string;

  constructor(private investimentoService: InvestimentoService) { }

  calcularInvestimento() {
    if (!this.valorInicial || !this.quantidadeMeses) {
      this.erro = 'Preencha todos os campos!';
      return;
    }

    const payload = {
      valorInicial: this.valorInicial, 
      quantidadeMeses: this.quantidadeMeses
    };

    console.log("Enviando payload:", payload);

    this.investimentoService.calcularInvestimento(payload).subscribe({
      next: (res) => {
        this.valorBruto = res.valorBruto;
        this.valorLiquido = res.valorLiquido;
        this.erro = undefined;
      },
      error: (err) => {

        var ErroEmQuantidadeMeses = err.error?.errors?.QuantidadeMeses;
        var ErroNoValorInicial = err.error?.errors?.ValorInicial;

        this.erro = ErroEmQuantidadeMeses ? ErroEmQuantidadeMeses?.[0] : " ";
        this.erro += ErroNoValorInicial ? ErroNoValorInicial?.[0]:"";
        console.error("Erro na requisição:", err);
      }
    });
  }
}
