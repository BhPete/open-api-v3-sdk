package okex

/*
 Get a http client
*/
func NewTestClient() *Client {
	// Set OKEX API's config
	var config Config
	config.Endpoint = "https://www.okex.com/"
	config.ApiKey = ""
	config.SecretKey = ""
	config.Passphrase = ""
	config.TimeoutSecond = 45
	config.IsPrint = true
	config.I18n = ENGLISH

	client := NewClient(config)
	return client
}
