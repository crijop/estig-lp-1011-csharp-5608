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
	 * Class DocentesDoutorados
	 * Vai conter 4 variaveis
	 * o nome do docente
	 * o grau, o ano e o
	 * estabelecimento do docente
	 * */
	public class DocentesDoutorados
	{
	    public string nomeDocente;
	    public string grau;
		public int ano;
		public string estabelecimento;
	
	    public DocentesDoutorados(string nomeDocente, string grau, int ano, string estabelecimento)
	    {
	        this.nomeDocente = nomeDocente;
			this.grau = grau;
			this.ano = ano;
			this.estabelecimento = estabelecimento;
	    }
	}
}
