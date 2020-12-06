using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}
		
		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			// Assert
			_driver.Url.Should().Contain("/Doacoes/Create");
		}

		[Fact]
		public void DoacaoUI_PreenchimentoDoeAgora()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			// Página Doe Agora
			_driver.FindElement(By.Id("valor")).SendKeys("50");

			_driver.FindElement(By.Id("DadosPessoais_Nome")).SendKeys(doacao.DadosPessoais.Nome);
			_driver.FindElement(By.Id("DadosPessoais_Email")).SendKeys(doacao.DadosPessoais.Email);
			_driver.FindElement(By.Id("DadosPessoais_Nome")).SendKeys(doacao.DadosPessoais.Nome);

			_driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco")).SendKeys(doacao.EnderecoCobranca.TextoEndereco);
			_driver.FindElement(By.Id("EnderecoCobranca_Numero")).SendKeys(doacao.EnderecoCobranca.Numero);
			_driver.FindElement(By.Id("EnderecoCobranca_Cidade")).SendKeys(doacao.EnderecoCobranca.Cidade);
			_driver.FindElement(By.Id("estado")).SendKeys(doacao.EnderecoCobranca.Estado);
			_driver.FindElement(By.Id("cep")).SendKeys(doacao.EnderecoCobranca.CEP);
			_driver.FindElement(By.Id("telefone")).SendKeys(doacao.EnderecoCobranca.Telefone);

			_driver.FindElement(By.Id("FormaPagamento_NomeTitular")).SendKeys(doacao.FormaPagamento.NomeTitular);
		  _driver.FindElement(By.Id("cardNumber")).SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);
			_driver.FindElement(By.Id("validade")).SendKeys(doacao.FormaPagamento.Validade);
			_driver.FindElement(By.Id("cvv")).SendKeys(doacao.FormaPagamento.CVV);

			// Assert
			_driver.Url.Should().Contain("/Doacoes/Create");
		}
	}
}