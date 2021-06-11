using System;

namespace site_cms.Models
{
  public class Pagina
  {
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Conteudo { get; set; }

    public bool AreaRestrita { get; set; }

    public bool Login { get; set; }
    
    public bool Home { get; set; }
    
    public int Ordem { get; set; }
  }
}