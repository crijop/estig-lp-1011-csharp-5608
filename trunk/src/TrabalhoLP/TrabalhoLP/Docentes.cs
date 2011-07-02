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
	 * Class Docentes
	 * Vai conter 3 variáveis
	 * o nome do docente,
	 * o estabelecimento do docente
	 * e o ano
	 * */
	public class Docentes
	{
	    public string nomeDocente;
		public string estabelecimento;
		public int ano;
	
	    public Docentes(string nomeDocente, string estabelecimento, int ano)
	    {
	        this.nomeDocente = nomeDocente;
			this.estabelecimento = estabelecimento;
			this.ano = ano;
	    }
	}
}
