/*
 * @Author:  Carlos Alexandre Rijo Palma Nº5608
 * @Curso:   Eng Informática Regime: Diurno
 * @Cadeira: Linguagens Programação 2ºANO -Terceiro trabalho (Csharp)
 * @Data:    20/06/2011
 */
using System;
using System.Data;
using System.IO;
using Mono.Data.SqliteClient;
using System.Collections.Generic;
using TrabalhoLP;
using System.Collections;

public class Rebides
{
	//Objecto da class EscreverFicheiro
	public static EscreverFicheiro wFile = new EscreverFicheiro("");
	
	/*
	 * Método Main
	 **/
	public static void Main(string[] args) 
	{
		string nomeBaseDados = "BaseDadosrebides.db";
		int anoDaEstatitica = 1;
		int anoAnterior = 0;
		int anoActual = 1;
		
		// chamar o metodo do numero total de docentes
		foreach(int ano in Range(0, 10))
		{
			numeroTotalDocentes(ano, nomeBaseDados);
		}
		
		//Número total de docentes por Estabelecimento
		numeroTotalDocEstab(1, nomeBaseDados);
		
		//Número total de docentes por grau
		foreach(int ano in Range(0, 10))
		{
			nrTotalDocGrau(ano, nomeBaseDados);
		}
		
		//Número total de docentes por grau e por estabelecimento
		foreach(int ano in Range(0, 10))
		{
			nrTotalDocGrauEstab(ano, nomeBaseDados);
		}
		
		//Número docentes Doutourados por estabelecimento e por ano
		numeroDoutoradosEstab(anoDaEstatitica, nomeBaseDados);
		
		//Conjunto de docentes doutorados por estabelecimento e por ano
		printListaDocentesDoutorados(anoDaEstatitica, nomeBaseDados);
		
		//Conjunto de docentes que mudaram de um estabelecimento 
		//para outro por ano
		conjuntoDocentesMudaramEstab(anoAnterior, anoActual, nomeBaseDados);
		
		//lista de Estabelecimentos por ano
		foreach(int ano in Range(0, 10))
		{
			printlistaEstabelecimentos(ano, nomeBaseDados);
		}
		
		//lista dos titulares de um grau por ano
		printListaTitularesGraus(anoDaEstatitica, nomeBaseDados);
		
		//Número de docentes que saem e entram numa instituiçao por ano
		docentesEntramSaemInstituicao(nomeBaseDados);

		//Número de docentes que migram de um estabelecimento
		//para outro por ano
		printNrDocentesMigramEstabParaEstab(anoAnterior, anoActual, nomeBaseDados);
		
		//Número de docentes promovidos para a próxima
		//categoria a cada ano por estabelecimento
		printNrDocPromovidosCatEstab(0, 1, nomeBaseDados);		
	}
	
	/*
	 * Função Range
	 * */
	public static IEnumerable<int> Range( int min, int max )
	{
   		for ( int i = min; i < max; i++ )
   		{
      		yield return i;
   		}
	}
	
	/*
	 * Metodo que lê e processa, dados através de consultas
	 * a uma Base dados de forma a obter o numero
     * total de docentes num determinado ano.
     * Recebe como parametro o ano a que se referem o 
     * numero de docentes e o nome da Base Dados. 
	 **/
	public static void numeroTotalDocentes(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO TOTAL DE DOCENTES NO ENSINO SUPERIOR NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		string queryNrDocentes = "SELECT count(DISTINCT docente) as NumeroDocentes from registo_docentes where ano = " + ano;
		dbcmd.CommandText = queryNrDocentes;
		IDataReader reader = dbcmd.ExecuteReader();
		
		while(reader.Read()) 
		{
		    string numeroDocentes = reader.GetString (0);
		    Console.WriteLine("Número Docentes no ensino Superior no ano 200" + ano + ": " +
		        numeroDocentes);
			
			//Escrever no ficheiro
			string menssagem = "Número Docentes no ensino Superior no ano 200" + ano + ": " +
		        numeroDocentes;
			wFile.escreverFicheiro(menssagem);
			
	   	}
		
		// clean up
		reader.Close();
		reader = null;
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
	}

	/*
	 * Metodo que lê e processa, dados através de consultas
	 * a uma Base dados de forma a obter o numero
     * total de docentes por estabelecimento num determinado ano.
     * Recebe como parametro o ano a que se referem o 
     * numero de docentes por estabelecimento
     * e o nome da Base Dados.
	 **/
	public static void numeroTotalDocEstab(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO TOTAL DE DOCENTES POR ESTABELECIMENTO NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		List<string> listaEstabelecimentos = listaEstab(ano, nomeBaseDados);
		int i = 0;
		foreach (string x in listaEstabelecimentos)
		{
			string queryNrDocEstab = "SELECT count (DISTINCT docente), estabelecimento, ano " + 
						"FROM registo_docentes WHERE ((estabelecimento) Like '" +
						x + "') and ano = " + ano;
			
			dbcmd.CommandText = queryNrDocEstab;
			IDataReader reader = dbcmd.ExecuteReader();
			
			i++;
			while(reader.Read()) 
			{
				
				string nrDocentes = reader.GetString (0);
				string nameEstab = reader.GetString (1);
				string anoQuery = reader.GetString (2);
				Console.WriteLine(i + ") No ano 200" + anoQuery + " O número total de docentes no estabelecimento " 
				                  + nameEstab + " é igual a " + nrDocentes + " Docentes");
				
				//Escrever no ficheiro
				string menssagem = i + ") No ano 200" + anoQuery + " O número total de docentes no estabelecimento " 
				                  + nameEstab + " é igual a " + nrDocentes + " Docentes";
				wFile.escreverFicheiro(menssagem);
	   		}
			// clean up
	   		reader.Close();
	   		reader = null;
		}
		
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;	
	}
	
	/*
	 * Metodo que lê e processa, dados através de consultas
	 * a uma Base dados de forma a obter o numero
     * total de docentes por grau num determinado ano.
     * Importa referir que este metodo só faz a contagem dos numeros de docentes 
 	 * para os graus Licenciados, Mestres e Doutoures.
     * Recebe como parametro o ano a que se referem o 
     * numero de docentes por grau
     * e o nome da Base Dados.
	 **/
	public static void nrTotalDocGrau(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO TOTAL DE DOCENTES POR GRAU NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		string[] listaGraus = { "%licenci%", "%mestre%", "do%" };
		string[] listaGrausPrint = { "Licenciados", "Mestres", "Doutorados" };
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		int i = 0;
		foreach (string grau in listaGraus)
		{
			string queryNrDocGrau = "SELECT count (DISTINCT (docente)), ano, grau " + 
            	            "FROM registo_docentes " +
                	        "WHERE grau LIKE '" + grau + "' AND " +  
                    	    "ano = " + ano ;
			dbcmd.CommandText = queryNrDocGrau;
			IDataReader reader = dbcmd.ExecuteReader();
			
			while(reader.Read()) 
			{
				string nrDocentes = reader.GetString (0);
				Console.WriteLine("No ano 200" + ano + " o número total de docentes com o grau " 
				                  + listaGrausPrint[i] + " é igual a " + nrDocentes + " Docentes");
				
				//Escrever no ficheiro
				string menssagem = "No ano 200" + ano + " o número total de docentes com o grau " 
				                  + listaGrausPrint[i] + " é igual a " + nrDocentes + " Docentes";
				wFile.escreverFicheiro(menssagem);
				
				i++;
			}
			// clean up
	   		reader.Close();
	   		reader = null;
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
	}
	
	/*
	 * Metodo que lê e processa, dados através de consultas
	 * a uma Base dados de forma a obter o numero
     * total de docentes por grau e por estabelecimento
     * num determinado ano.
     * Importa referir que este metodo só faz a contagem dos numeros de docentes 
 	 * para os graus Licenciados, Mestres e Doutoures.
     * Recebe como parametro o ano a que se referem o 
     * numero de docentes por grau
     * e o nome da Base Dados.
	 **/
	public static void nrTotalDocGrauEstab(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO TOTAL DE DOCENTES POR GRAU E POR ESTABELECIMENTO NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		string[] listaGraus = { "%licenci%", "%mestre%", "do%" };
		string[] listaGrausPrint = { "Licenciados", "Mestres", "Doutorados" };
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		int i = 0;	
		foreach(string grau in listaGraus)
		{
			foreach(string estab in listaEstab(ano, nomeBaseDados))
			{
				string queryNrDocGrauEst = "SELECT count (DISTINCT (docente)), ano, grau, estabelecimento " +
            		            "FROM registo_docentes " +
                		        "WHERE grau LIKE '" + grau + "' AND " +
                    		    "estabelecimento LIKE '" + estab + "' AND " + 
                        		"ano = " + ano ;
				dbcmd.CommandText = queryNrDocGrauEst;
				IDataReader reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) 
				{
					string nrDocentes = reader.GetString (0);
					Console.WriteLine("No ano 200" + ano + " o número total de docentes com o grau " 
					                  + listaGrausPrint[i] + " no estabelecimento " + estab 
					                  + " é igual a " + nrDocentes + " Docentes");
					
					//Escrever no Ficheiro
					string menssagem = "No ano 200" + ano + " o número total de docentes com o grau " 
					                  + listaGrausPrint[i] + " no estabelecimento " + estab 
					                  + " é igual a " + nrDocentes + " Docentes";
					wFile.escreverFicheiro(menssagem);
					
					i++;
					if(i > 2)
					{
						i = 0;		
					}
				}
				// clean up
				reader.Close();
				reader = null;
			}
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
	}
	
	/*
	 * Metodo que devolve o numero total de instituições em
	 * cada ano. Recebe como parametro o ano a que se refere e o 
	 * nome da base dados.
	 **/
	public static int numeroInstituicoes(int ano, string nomeBaseDados)
	{
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		string queryNrInstituicoes = "SELECT DISTINCT (tipo_estabelecimento) AS Número_Instituições  FROM registo_docentes WHERE ano = " + ano;
		dbcmd.CommandText = queryNrInstituicoes;
		IDataReader reader = dbcmd.ExecuteReader();
		
		int nrInstituicoes = 0;
		while(reader.Read()) 
		{
		   	nrInstituicoes++;
	   	}
		// clean up
		reader.Close();
		reader = null;
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return nrInstituicoes;
	}
		
	/*
	 * Metodo que vai dar o numero de docentes que entraram e sairam de 
	 * uma instituição de uns anos para os outros.
	 * O metodo também escreve num ficheiro de texto a estatistica.
	 **/ 
	public static void docentesEntramSaemInstituicao(string nomeBaseDados)
	{	
		//Titulo da estatistica
		string tituloEstatistica = "\nDOCENTES QUE ENTRARAM/SAIRAM DE CADA INSTITUIÇÃO POR ANO \n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		int numeroDocentes = 0;
		int contador = 0;
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		int ano;
		string nomeInstit = "";
		for (int i = 0; i < 5; i++)
		{
			for(ano = 0; ano < 10; ano ++)
			{
				List<string> listaTipoEstabelecimentos = listaTipoEstab(ano, nomeBaseDados);
				string queryNumeroDocente = "SELECT DISTINCT (docente) as NumeroDocentes" +
						" from registo_docentes where tipo_estabelecimento LIKE '" +
						listaTipoEstabelecimentos[i] + "' AND ano = " + ano;
				dbcmd.CommandText = queryNumeroDocente;
				IDataReader reader = dbcmd.ExecuteReader();
				
				int nrDocentes = 0;
				string menssagem;
				while(reader.Read()) 
				{
				  	nrDocentes ++;
			   	}
				if(ano == 0 )
				{	
					contador = 0;
					numeroDocentes = nrDocentes;
					nomeInstit = listaTipoEstabelecimentos[i];
					Console.WriteLine(String.Format("\nNo ano 200{0} o número de docentes na instituição {1} é = {2}\n", 
					                                ano, nomeInstit, numeroDocentes));
					//Escrever no Ficheiro
					menssagem = String.Format("\nNo ano 200{0} o número de docentes na instituição {1} é = {2}\n", 
					                                ano, nomeInstit, numeroDocentes);
					wFile.escreverFicheiro(menssagem);
					contador++;
					
				}
				if(ano == contador)
				{
					if(numeroDocentes > nrDocentes && nomeInstit == listaTipoEstabelecimentos[i])
					{
						numeroDocentes = numeroDocentes - nrDocentes;
						Console.WriteLine(String.Format("Do ano 200{0} para o ano 200{1} -> Sairam da instituição {2}, {3} docentes",
					       							   (contador - 1), contador, nomeInstit, numeroDocentes));
						//Escrever no Ficheiro
						menssagem = String.Format("Do ano 200{0} para o ano 200{1} -> Sairam da instituição {2}, {3} docentes",
					       							   (contador - 1), contador, nomeInstit, numeroDocentes);
						wFile.escreverFicheiro(menssagem);
						
						numeroDocentes = nrDocentes;
						nomeInstit = listaTipoEstabelecimentos[i];
						contador++;
					}
					else if(numeroDocentes < nrDocentes && nomeInstit == listaTipoEstabelecimentos[i])
					{
						numeroDocentes = nrDocentes - numeroDocentes;
						Console.WriteLine(String.Format("Do ano 200{0} para o ano 200{1} -> Entraram na instituição {2}, {3} docentes",
					       							   (contador - 1), contador, nomeInstit, numeroDocentes));
						//Escrever no Ficheiro
						menssagem = String.Format("Do ano 200{0} para o ano 200{1} -> Entraram na instituição {2}, {3} docentes",
					       							   (contador - 1), contador, nomeInstit, numeroDocentes);
						wFile.escreverFicheiro(menssagem);
						
						numeroDocentes = nrDocentes;
						nomeInstit = listaTipoEstabelecimentos[i];
						contador++;
					}
					/*else if(numeroDocentes == 0 && nomeInstit != listaTipoEstabelecimentos[i])
					{
						Console.WriteLine(String.Format("A Instituição {0} fechou no ano 200{1}",
					       nomeInstit, (contador - 1)));
						numeroDocentes = nrDocentes;
						contador++;
					}*/
				}
				// clean up
				reader.Close();
				reader = null;
			}
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
	}
	
	/*
	 * Metodo que lê e processa, dados através de consultas
	 * a uma Base dados de forma a obter o numero
     * total de docentes Doutorados em cada estabelecimento
     * num determinado ano.
     * Recebe como parametro o ano a que se referem o 
     * numero de docentes e o nome da Base Dados.
     * o método escreve a estatistica num ficheiro de texto 
	 **/
	public static void numeroDoutoradosEstab(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO TOTAL DE DOCENTES DOUTORADOS NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		List<string> listaEstabelecimentos = listaEstab(ano, nomeBaseDados);
		string doutorados = "do%";
		
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
			
		foreach(string estabelecimento in listaEstabelecimentos)
		{
			string queryNrDocEstab = "SELECT DISTINCT (docente), estabelecimento, grau, ano " +
								"FROM registo_docentes WHERE grau LIKE '" + doutorados + "' AND estabelecimento LIKE '" +
								estabelecimento + "' AND ano = " + ano;
	
			dbcmd.CommandText = queryNrDocEstab;
			IDataReader reader = dbcmd.ExecuteReader();
			
			int nrDocEstab = 0;
			while(reader.Read()) 
			{
				nrDocEstab++;
	   		}
			Console.WriteLine(String.Format("No ano 200{0} o estabelecimento {1} tem {2} docentes doutorados"
				                                , ano, estabelecimento, nrDocEstab));
			//Escrever no Ficheiro
			string menssagem = String.Format("No ano 200{0} o estabelecimento {1} tem {2} docentes doutorados"
				                                , ano, estabelecimento, nrDocEstab);
			wFile.escreverFicheiro(menssagem);
			
			// clean up
			reader.Close();
			reader = null;
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
	}
		
	/*
	 * _____________Listas_________________
	 * */
	
	/*
	 * metodo que devolve uma lista com os estabelecimentos num dado ano,
     * devolve o nome de estabelecimento
     * recebe como parametro o nome da base dados e
     * o ano a que se rerfer.
     * Importa ainda que referir que antes de devlover a lista
     * faz ainda a ordenação dessa lista através da função Sort
	 * */
	public static List<string> listaEstab(int ano, string nomeBaseDados)
	{
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		string queryEstab = "SELECT DISTINCT (estabelecimento), ano FROM registo_docentes WHERE ano = " + ano;
		dbcmd.CommandText = queryEstab;
		IDataReader reader = dbcmd.ExecuteReader();
		
		List<string> listaEstabelecimentos = new List<string>();
		
		while(reader.Read()) 
		{
		    string nameEstab = reader.GetString (0);
			listaEstabelecimentos.Add(nameEstab);
	   	}
		//ordenar lista de estabelecimentos
		listaEstabelecimentos.Sort(delegate(string nome1, string nome2) { return nome1.CompareTo(nome2); });
		
		// clean up
	   	reader.Close();
	   	reader = null;
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;	
		return listaEstabelecimentos;
	}
	
	/*
	 * metodo que devolve uma lista com o tipo de estabelecimentos
	 * num dado ano, devolve o nome do tipo de estabelecimento
     * recebe como parametro o nome da base dados e
     * o ano a que se rerfer.
     * Importa ainda que referir que antes de devlover a lista
     * faz ainda a ordenação dessa lista através da função Sort
	 * */
	public static List<string> listaTipoEstab(int ano, string nomeBaseDados)
	{
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		string queryEstab = "SELECT DISTINCT (tipo_estabelecimento), ano FROM registo_docentes WHERE ano = " + ano;
		dbcmd.CommandText = queryEstab;
		IDataReader reader = dbcmd.ExecuteReader();
		
		List<string> listaTipoEstabelecimentos = new List<string>();
		while(reader.Read()) 
		{
		    string nameTipoEstab = reader.GetString (0);
			listaTipoEstabelecimentos.Add(nameTipoEstab);
	   	}
		//ordenar lista de estabelecimentos
		listaTipoEstabelecimentos.Sort(delegate(string nome1, string nome2) { return nome1.CompareTo(nome2); });
		
		// clean up
		reader.Close();
		reader = null;
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return listaTipoEstabelecimentos;
	}
	
	/*
	 * metodo que devolve uma lista com os titulares dos graus num dado ano.
	 * Importa referir que é uma lista de TitularesGraus, que foi uma class 
	 * criada aparte, que vai conter o nome do docente, o grau e o ano.
     * recebe como parametro o nome da base dados e
     * o ano a que se rerfer.
     * os graus Licenciados, Mestres e Doutoures.
	 * */
	public static List<TitularesGraus> listaTitularesGraus(int ano, string nomeBaseDados)
	{
		string[] listaGraus = { "%licenci%", "%mestre%", "do%" };
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		
		List<TitularesGraus> lTitularesGraus = new List<TitularesGraus>();
		
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		foreach(string grau in listaGraus)
		{
			string queryTitularesGraus = "SELECT DISTINCT (docente), grau, ano" +
            	        " FROM registo_docentes WHERE grau LIKE '" + grau +
                	    "' AND ano = " + ano;
			//Console.WriteLine(queryTitularesGraus);
			
			dbcmd.CommandText = queryTitularesGraus;
			IDataReader reader = dbcmd.ExecuteReader();
			
			while(reader.Read()) 
			{
			    string nameDocente = reader.GetString (0);
				string tipoGrau = reader.GetString (1);
				string anoGrau = reader.GetString(2);
				lTitularesGraus.Add(new TitularesGraus(nameDocente, tipoGrau, anoGrau));
	   		}
			// clean up
			reader.Close();
			reader = null;
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return lTitularesGraus;
	}
	
	/*
	 * metodo que devolve uma lista com os docentes doutorados num dado ano.
	 * Importa referir que é uma lista de DocentesDoutorados, que foi uma class 
	 * criada aparte, que vai conter o nome do docente, o grau, o ano e o estabelecimento.
     * recebe como parametro o nome da base dados e
     * o ano a que se rerfer.
     * Importa referir que esta estatistica é ordenada pelo nome do 
     * docente através da função Sort.
	 * */
	public static List<DocentesDoutorados> listaDocentesDoutorados(int ano, string nomeBaseDados)
	{
		string doutorados = "do%";
		List<string> listaEstabelecimentos = listaEstab(ano, nomeBaseDados);
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		
		List<DocentesDoutorados> ldocentesDouto = new List<DocentesDoutorados>();
		
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		foreach(string estabelecimento in listaEstabelecimentos)
		{
			string queryNrDocEstab = "SELECT DISTINCT (docente), estabelecimento, grau, ano " +
								"FROM registo_docentes WHERE grau LIKE '" + doutorados + "' AND estabelecimento LIKE '" +
								estabelecimento + "' AND ano = " + ano;
			
			dbcmd.CommandText = queryNrDocEstab;
			IDataReader reader = dbcmd.ExecuteReader();
			
			//int nrDocEstab = 0;
			while(reader.Read()) 
			{
				string nomeDocente = reader.GetString (0);
				string grau = reader.GetString (2);
			    string estab = reader.GetString (1);
				ldocentesDouto.Add(new DocentesDoutorados(nomeDocente, grau, ano, estab));
				
	   		}
			// clean up
			reader.Close();
			reader = null;
		}
		//sort
		ldocentesDouto.Sort(delegate(DocentesDoutorados x, DocentesDoutorados y)
		{ 
			return x.nomeDocente.CompareTo(y.nomeDocente); 
		});
		
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return ldocentesDouto;
	}
	
	/*
	 * Método que devolve uma lista de Docentes, lista esta que 
	 * vai conter o nome do docente, o estabelecimento que frequenta e o ano
	 * a que se refere.
	 * Este método vai receber como parametro o ano desejado da lista de docentes
	 * e o nome da base dados.
	 * */
	public static List<Docentes> listaDocentes(int ano, string nomeBaseDados)
	{
		List<Docentes> ldocentes = new List<Docentes>();
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
	
		List<string> listaEstabelecimentos = listaEstab(ano, nomeBaseDados);
		foreach(string estabelecimento in listaEstabelecimentos)
		{
			string queryDocEstab = "SELECT DISTINCT (docente), estabelecimento, ano " +
								"FROM registo_docentes WHERE estabelecimento LIKE '" +
								estabelecimento + "' AND ano = " + ano;
			dbcmd.CommandText = queryDocEstab;
			IDataReader reader = dbcmd.ExecuteReader();
			while(reader.Read()) 
			{
				string nomeDocente = reader.GetString (0);
			    string estab = reader.GetString (1);
				ldocentes.Add(new Docentes(nomeDocente, estab, ano));
			}
			// clean up
			reader.Close();
			reader = null;
		}
		// clean up
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return ldocentes;
	}
	
	/*
	 * Método que devolve uma lista de Docentes, lista esta que 
	 * vai conter os docentes que mudaram de Estabelecimento de um
	 * ano para o outro.
	 * Este método recebe como parametro o anoAnterior, isto é, o
	 * ano em que vai dizer o estabelecimento que o docente frequentava,
	 * recebe também o anoActual que vai dizer o estabelecimento que o 
	 * docente frequenta actualmente e por fim recebe o nome da base dados.
	 * */
	public static List<Docentes> conjuntoDocentesMudaramEstab(
                 int anoAnterior, int anoActual, string nomeBaseDados)
	{
		List<Docentes> listaDocEstabAnterior = listaDocentes(anoAnterior, nomeBaseDados);
		List<Docentes> listaDocEstabActual = listaDocentes(anoActual, nomeBaseDados);
		List<Docentes> listaNova = new List<Docentes>();
		foreach(Docentes x in listaDocEstabAnterior)
		{
			foreach(Docentes y in listaDocEstabActual)
			{
				if(x.nomeDocente.Equals(y.nomeDocente))
				{
						if(!x.estabelecimento.Equals(y.estabelecimento))
						{
							listaNova.Add(new Docentes(x.nomeDocente, y.estabelecimento, y.ano));
						}
				}
			}
		}
		return listaNova;
	}
	
	/*
	 * Método que devolve uma lista, lista essa que vai conter
	 * o nome do docente, a sua categoria, o estabelecimento que 
	 * frequenta e o ano a que se refere.
	 * De referir que a lista é do tipo de DocentesCategoria, 
	 * uma class criada conter os dados anteriormente referidos.
	 * Este método recebe como parametro o ano a que se refere
	 * e o nome da base de dados onde a consulta vai ser feita.
	 * */
	public static List<DocentesCategoria> listaDocentesCategoriaEstabAno(int ano, string nomeBaseDados)
	{
		List<DocentesCategoria> docCat = new List<DocentesCategoria>();
		string connectionString = "URI=file:" + nomeBaseDados + ", version = 3";
		IDbConnection dbcon;
		dbcon = (IDbConnection) new SqliteConnection(connectionString);
		dbcon.Open();
		IDbCommand dbcmd = dbcon.CreateCommand();
		
		string queryDocentes = "SELECT DISTINCT (docente), categoria, estabelecimento, ano from registo_docentes where ano = " + ano;
		dbcmd.CommandText = queryDocentes;
		IDataReader reader = dbcmd.ExecuteReader();
		
		while(reader.Read()) 
		{
		    string nomedocente = reader.GetString (0);
			string categoria = reader.GetString (1);
			string estabelecimento = reader.GetString(2);
			docCat.Add(new DocentesCategoria(nomedocente, categoria, estabelecimento, ano));
	   	}
		// clean up
		reader.Close();
		reader = null;
	   	dbcmd.Dispose();
	   	dbcmd = null;
	   	dbcon.Close();
	   	dbcon = null;
		return docCat;
	}
	
	/*
	 * Método que devolve um dicionário, com a chave string e o
	 * valor int. A chave vai corresponder ao estabelecimento e o valor
	 * corresponde ao numero de docentes que frequenta esse estabelecimento 
	 * e que foram promovidos na sua categoria.
	 * Recebe como parâmetro o anoAnterior para ir ver a categoria que o 
	 * docente tinha, recebe também o anoActual para ir ver se o docente mudou 
	 * ou não de categoria e por fim recebe o nome da base de dados.
	 * */
	public static Dictionary<string, int> nrDocPromovidosCatEstab(int anoAnterior, int anoActual, string nomeBaseDados)
	{
		List<DocentesCategoria> listDocCategoria = listaDocentesCategoriaEstabAno(anoAnterior, nomeBaseDados);
		List<DocentesCategoria> listDodPromovidos = listaDocentesCategoriaEstabAno(anoActual, nomeBaseDados);
		List<DocentesCategoria> listaNova = new List<DocentesCategoria>();
		
		//Docentes que mudaram de Categoria de um ano para outro
		foreach (DocentesCategoria y in listDodPromovidos)
		{
			foreach(DocentesCategoria x in listDocCategoria)
			{
				if(y.nomeDocente.Equals(x.nomeDocente))
				{
					if(!y.categoria.Equals(x.categoria))
					{
						listaNova.Add(new DocentesCategoria(y.nomeDocente, y.categoria, y.estabelecimento, y.ano));
						//Console.WriteLine(z.nomeDocente + " " + x.categoria + " " + z.ano);			
					}
				}
			}
		}
		//Ordenar a nova lista<DocentesCategoria> 
		listaNova.Sort(delegate(DocentesCategoria x, DocentesCategoria y)
		{ 
			return x.estabelecimento.CompareTo(y.estabelecimento); 
		});
		//nova lista de estabelecimentos
		//são todos os estabelecimentos que contém docentes 
		//que mudaram de categoria
		List<string> listaEstabelecimentos = new List<string>();
		foreach (DocentesCategoria z in listaNova)
		{
			listaEstabelecimentos.Add(z.estabelecimento);
		}
		
		//Remover estabelecimentos Duplicados
		listaEstabelecimentos = removerDadosRepetidos(listaEstabelecimentos);
		
		//ordenar lista de estabelecimentos
		listaEstabelecimentos.Sort(delegate(string nome1, string nome2) { return nome1.CompareTo(nome2); });
				
		//Criação do dicionário
		//este dicionário vai ter como chave o nome dos estabelecimentos
		//que contém docentes que mudaram de categoria
		//e vai ter como valor o número de docentes que mudaram de categoria
		//nesse estabelecimento
		Dictionary<string, int> dicNrDocentesEstab = new Dictionary<string, int>();
		foreach(string estabelecimento in listaEstabelecimentos )
		{	
			int nrDocentes = 0;
			foreach(DocentesCategoria z in listaNova)
			{
				if(estabelecimento.Equals(z.estabelecimento))
				{
					nrDocentes++;
				}
			}
			//adiciona elementos ao dicionário
			dicNrDocentesEstab.Add(estabelecimento, nrDocentes);
		}
		//retorna o dicionário
		return dicNrDocentesEstab;
	}
	
	/*
	 * Métodos para imprimir as listas
	 **/
	
	/*
	 * Metodo elaborado para fazer o print da lista de estabelecimentos
	 * na consola. 
	 * recebe a lista de estabelecimentos através do metodo "listaEstab".
	 * o metodo recebe como parametros o ano a k se refer a estatistica e o
	 * nome da base dados.
	 * Importa ainda referir que este metodo é responsavél por escrever 
	 * a estatistica no ficheiro de texto.
	 * */
	public static void printlistaEstabelecimentos(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nLISTA DE ESTABELECIMENTOS NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		List<string> listaEstabelecimentos = listaEstab(ano, nomeBaseDados);
		int i = 1;
		listaEstabelecimentos.ForEach(delegate(string estabelcimento) 
		{
			Console.WriteLine(i + ") " + estabelcimento);
			//Escrever no ficheiro
			string menssagem = i + ") " + estabelcimento;
			wFile.escreverFicheiro(menssagem);
			i++;
		});
	}
	
	/*
	* Metodo elaborado para fazer o print da lista dos titulares dos
	* graus na consola. 
	* recebe a lista dos titulares do graus através do metodo 
	* "listaTitularesGraus".
	* o metodo recebe como parametros o ano a k se refer a estatistica e o
	* nome da base dados.
	* Importa ainda referir que este metodo é responsavél por escrever 
	* a estatistica no ficheiro de texto.
	* */	
	public static void printListaTitularesGraus(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nLISTA DOS TITULARES DOS GRAUS NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		List<TitularesGraus> lTitulGrau = listaTitularesGraus(ano, nomeBaseDados);
		//sort
		lTitulGrau.Sort(delegate(TitularesGraus x, TitularesGraus y){return x.nomeDocente.CompareTo(y.nomeDocente);});
		int i = 0;
		lTitulGrau.ForEach(delegate(TitularesGraus x)
		{
			i++;
			Console.WriteLine(String.Format(i + ") No ano 200{0} o docente {1} contém o grau {2} "
			                               	, x.ano, x.nomeDocente, x.grau));
			//Escrever no Ficheiro
			string menssagem = String.Format(i + ") No ano 200{0} o docente {1} contém o grau {2} "
			                               	, x.ano, x.nomeDocente, x.grau);
			wFile.escreverFicheiro(menssagem);			
		});
	}
	
	/*
	 * Metodo elaborado para fazer o print da lista dos docentes
	 * doutorados em cada ano. 
	 * recebe a lista de docentes doutorados através do metodo 
	 * "listaDocentesDoutorados".
	 * o metodo recebe como parametros o ano a k se refer a lista e o
	 * nome da base dados.
	 * Importa ainda referir que este metodo é responsavél por escrever 
	 * a estatistica no ficheiro de texto.
	 **/	
	public static void printListaDocentesDoutorados(int ano, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nLISTA DE DOCENTES DOUTORADOS NO ANO 200" + ano + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		List<DocentesDoutorados> ldocentesDouto = listaDocentesDoutorados(ano, nomeBaseDados);
		ldocentesDouto.ForEach(delegate(DocentesDoutorados x)
		{
			Console.WriteLine(String.Format("No ano {0} o docente {1} contém o grau {2} estab {3} "
			                               	, x.ano, x.nomeDocente, x.grau, x.estabelecimento));
			//Escrever no Ficheiro
			string menssagem = String.Format("No ano {0} o docente {1} contém o grau {2} estab {3} "
			                               	, x.ano, x.nomeDocente, x.grau, x.estabelecimento);
			wFile.escreverFicheiro(menssagem);
		});
	}
	
	/*
	 * Metodo elaborado para fazer o print do dicionario
	 * com o número de docentes promovidos para a próxima
	 * categoria por estabelecimento.
	 * recebe o dicionário com a chave que é os estabelecimentos
	 * e o valor que é o numero de docentes promovidos em cada 
	 * estabelecimento.  
	 * Metodo que retorna o dicionário é "nrDocPromovidosCatEstab".
	 * o metodo recebe como parametros o anoAnterior para ver a categoria do docente,
	 * recebe também o anoActual para ver se o docente mudou ou nao a sua categoria e 
	 * por fim o nome da base de dados.
	 * Importa ainda referir que este metodo é responsavél por escrever 
	 * a estatistica no ficheiro de texto.
	 **/
	public static void printNrDocPromovidosCatEstab(int anoAnterior, int anoActual, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\n NÚMERO DE DOCENTES PROMOVIDOS DO ANO 200" + anoAnterior +
								   " PARA O ANO 200" + anoActual + " POR ESTABELECIMENTO";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		Dictionary<string, int> dicNrDocentesEstab = nrDocPromovidosCatEstab(anoAnterior,
		                                                                     anoActual,
		                                                                  nomeBaseDados);
		foreach(KeyValuePair<string, int> x in dicNrDocentesEstab)
		{
			Console.WriteLine("O Número de Docentes promovidos " +
			                  "no estabelecimeno " + x.Key + " é igual a " + x.Value);
			
			//Escrever no Ficheiro
			string menssagem = String.Format("O Número de Docentes promovidos " +
			                  "no estabelecimeno " + x.Key + " é igual a " + x.Value);
			wFile.escreverFicheiro(menssagem);
		}
	}
	
	/*
	 * Método que faz o print do número de docentes que migram de
	 * um estabelecimento para outro dado o anoAnterior, ano para 
	 * ver o estabelecimento que o docente frequentava anteriormente,
	 * dado o anoActual, ano para ver o estabelecimento que o docente está
	 * a frequentar e dado também o nome da base dados.
	 * Importa referir que este método recebe uma lista dos docentes
	 * que mudaram de estabelecimento, através do método "conjuntoDocentesMudaramEstab".
	 * De referir também que este método é o responsável de escrever esta estatistica
	 * no ficheiro de texto.
	 * */
	public static void printNrDocentesMigramEstabParaEstab(
	                       int anoAnterior, int anoActual, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nNÚMERO DE DOCENTES QUE MIGRAM DE UM ESTABELECIMENTO" +
			" PARA OUTRO DO ANO 200" + anoAnterior + " PARA O ANO 200" + anoActual + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		List<Docentes> listDocentesMudaramEstab = conjuntoDocentesMudaramEstab(
		                                 anoAnterior, anoActual, nomeBaseDados);
		int nrDoc = listDocentesMudaramEstab.Count;
		Console.WriteLine("Número de docentes que migraram de" +
					" um estabelecimento para outro do ano 200" + anoAnterior + 
		    		" para o ano 200" + anoActual + " é igual a " + nrDoc);
	
		//Escrever no ficheiro
		string menssagem = "Número de docentes que migraram de" +
					" um estabelecimento para outro do ano 200" + anoAnterior + 
		    		" para o ano 200" + anoActual + " é igual a " + nrDoc; 
		wFile.escreverFicheiro(menssagem);
	}
	
	/*
	 * Metodo elaborado para fazer o print da lista de 
	 * docentes que mudaram de estabelecimento de um
	 * ano para o outro. 
	 * Metodo que retorna essa lista é "conjuntoDocentesMudaramEstab".
	 * o metodo recebe como parametros o anoAnterior para ver o estabelecimento
	 * que o docente frequentava, recebe também o anoActual para ver o estabelecimento
	 * que o docente frequenta actualmente e por fim o nome da base de dados.
	 * Importa ainda referir que este metodo é responsavél por escrever 
	 * a estatistica no ficheiro de texto.
	 **/
	public static void printConjuntoDocentesMudaramEstab(
	              int anoAnterior, int anoActual, string nomeBaseDados)
	{
		//Titulo da estatistica
		string tituloEstatistica = "\nCONJUNTO DE DOCENTES QUE MUDARAM DE ESTABELECIMENTO" + 
							" DO ANO 200" + anoAnterior + "PARA O ANO" + anoActual + "\n";
		Console.WriteLine(tituloEstatistica);
		wFile.escreverFicheiro(tituloEstatistica);
		
		List<Docentes> DocentesMudaramEstab = conjuntoDocentesMudaramEstab(
		                                            anoAnterior, anoActual, nomeBaseDados);
		foreach(Docentes x in DocentesMudaramEstab)
		{
			Console.WriteLine("O docente " + x.nomeDocente + " mudou de estabelecimento");
			//Escrever no Ficheiro
			string menssagem = String.Format("O docente " + x.nomeDocente + " mudou de estabelecimento");
			wFile.escreverFicheiro(menssagem);
		}
	}
	
	/*
	 * Método criado para eliminar dados repetidos numa lista
	 * de strings.
	 * Este Método recebe como parâmetro uma lista de strings
	 * (List<string>) e devolve uma lista de strings já sem dados 
	 * repetidos.
	 * */
	public static List<string> removerDadosRepetidos(List<string> lista)
	{
	    Dictionary<string, int> dici = new Dictionary<string, int>();
	    List<string> listaSaida = new List<string>();
	    foreach (string dado in lista)
	    {
	        if (!dici.ContainsKey(dado))
	        {
	            dici.Add(dado, 0);
	            listaSaida.Add(dado);
	        }
	    }
	    return listaSaida;
	}
	
	/* Método criado para testar o método que
	 * remove os dados repetidos de uma lista
	 * de string 
	 * */
	public static void testarRemover()
	{
		List<string> l = new List<string>();
		l.Add("carlos");
		l.Add("5608");
		l.Add("Alexandre");
		l.Add("Rijo");
		l.Add("Rijo");
		l.Add("carlos");
		l.Add("5608");
		foreach(string x in l)
		{
			Console.WriteLine(x);
		}
		Console.WriteLine("\n\n");
		List<string> lRemodelada = removerDadosRepetidos(l);
		foreach(string y in lRemodelada)
		{
			Console.WriteLine(y);
		}
	}
}