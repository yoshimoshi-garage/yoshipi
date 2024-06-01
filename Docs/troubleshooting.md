# Troubleshooting

## GPIO Not Accessible

If you receive this error when attempting to access GPIO:
```
Meadow.NativeException: Failed to request line
```

it's likely that you've not configured your device to use both SPI buses.

