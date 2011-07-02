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
	 * Class DocentesCategoria
	 * Vai conter 4 variáveis
	 * o nome do docente, a
	 * categoria, o estabelecimento
	 * do docente e o ano
	 * */
	public class DocentesCategoria
	{
	    public string nomeDocente;
		public string categoria;
		public string estabelecimento;
		public int ano;
	
	    public DocentesCategoria(string nomeDocente, string categoria, string estabelecimento, int ano)
	    {
	        this.nomeDocente = nomeDocente;
			this.categoria = categoria;
			this.estabelecimento = estabelecimento;
			this.ano = ano;
	    }
	}
}
