// Описываем тип данных на входе вершинного шейдера
struct VertexShaderInput
{
    float4 position : POSITION;
};

// Описываем тип данных на входе пиксельного шейдера
struct PixelShaderInput
{
    float4 position : SV_POSITION;
};

// Вершинный шейдер
// на входе вершина из буфера
// на выходе вершина обработанная, готовая для пиксельного шейдера
PixelShaderInput SimpleVertexShader( VertexShaderInput input )
{
    PixelShaderInput output = (PixelShaderInput)0;
    
    // не делаем с вершиной ничего
    output.position = input.position;
    
    return output;
}

// Пиксельный шейдер
// на входе вершина обработанная вершинным шейдером
// на выходе цвет результирующего пикселя, в виде RGBA
float4 RedPixelShader( PixelShaderInput input ) : SV_Target
{
    // Возвращаем красный цвет для любого пикселя
    return float4(1.0, 0.0, 0.0, 0.0);
}

// определяем последовательность шейдеров для нашего эффекта
technique10 SimpleRedRender
{
    // первый проход, он же последний
    pass P0
    {
        // устанавливаем вершинный шейдер с компиляцией под Shader Model 4.0
        SetVertexShader( CompileShader( vs_4_0, SimpleVertexShader() ) );

        // устанавливаем пиксельный шейдер с компиляцией под Shader Model 4.0
        SetPixelShader( CompileShader( ps_4_0, RedPixelShader() ) );
    }
}