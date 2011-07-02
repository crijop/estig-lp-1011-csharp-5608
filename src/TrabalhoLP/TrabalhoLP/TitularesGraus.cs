/*
 * @Author:  Carlos Alexandre Rijo Palma Nº5608
 * @Curso:   Eng Informática Regime: Diurno
 * @Cadeira: Linguagens Programação 2ºANO -Terceiro trabalho (Csharp)
 * @Data:    20/06/2011
 */
using System;

namespace TrabalhoLP
{
	/*
	 * Class titulares dos graus
	 * Vai conter 3 variaveis
	 * o nome do docente
	 * o grau e o ano
	 * */
	public class TitularesGraus
	{
		public string nomeDocente;
   		public string grau;
		public string ano;
	
		public TitularesGraus(string nomeDocente, string grau, string ano)
	    {
	        this.nomeDocente = nomeDocente;
			this.grau = grau;
			this.ano = ano;
	    }
	}
}
