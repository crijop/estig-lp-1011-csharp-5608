/*
 * @Author:  Carlos Alexandre Rijo Palma Nº5608
 * @Curso:   Eng Informática Regime: Diurno
 * @Cadeira: Linguagens Programação 2ºANO -Terceiro trabalho (Csharp)
 * @Data:    20/06/2011
 */
using System;
using System.IO;

namespace TrabalhoLP
{
	/*
	 * Class EscreverFicheiro
	 * Vai conter 1 variável
	 * a menssagem, que é o que é 
	 * enviado para o ficheiro
	 * */
	public class EscreverFicheiro
	{
		public string menssagem;
		
		public EscreverFicheiro (string menssagem)
		{
			this.menssagem = menssagem;
		}
		
		/*
		 * Método que cria o ficheiro, se ainda não
		 * existir, se existir, vai acrescentar ao ficheiro
		 * o que for enviado para o ficheiro através 
		 * da variavél que recebe, a menssagem.
		 * */
		public void escreverFicheiro(string menssagem)
		{
			FileInfo file = new FileInfo("EstatisticaLP_C#.txt");
			StreamWriter Txt = file.AppendText();
			Txt.WriteLine(menssagem);
			Txt.Close();
		}
	}
	/*System.IO.StreamWriter file = new System.IO.StreamWriter("Estatisticas.txt");
	createFileListEA(file);
		file.Close();*/
}