package okex

/*
 OKEX api config info
 @author Tony Tian
 @date 2018-03-17
 @version 1.0.0
*/

type Config struct {
	// Rest api endpoint url. eg: http://www.okex.com/
	Endpoint string
	// The user's api key provided by OKEx.
	ApiKey string
	// The user's secret key provided by OKEx. The secret key used to sign your request data.
	SecretKey string
	// The Passphrase will be provided by you to further secure your API access.
	Passphrase string
	// Http request timeout.
	TimeoutSecond int
	// Whether to print API information
	IsPrint bool
	// Internationalization @see file: constants.go
	I18n string
}
